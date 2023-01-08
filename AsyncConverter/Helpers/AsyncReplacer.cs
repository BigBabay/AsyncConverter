using System.Linq;
using AsyncConverter.AsyncHelpers.AwaitEliders;
using AsyncConverter.Checkers.AsyncWait;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class AsyncReplacer : IAsyncReplacer
    {
        private readonly IAsyncInvocationReplacer asyncInvocationReplacer;
        private readonly IInvocationConverter invocationConverter;
        private readonly IAwaitElider awaitElider;
        private readonly IAwaitEliderChecker awaitEliderChecker;
        private readonly ISyncWaitChecker syncWaitChecker;
        private readonly ISyncWaitConverter syncWaitConverter;

        public AsyncReplacer(IAsyncInvocationReplacer asyncInvocationReplacer, IInvocationConverter invocationConverter,
                             IAwaitElider awaitElider, IAwaitEliderChecker awaitEliderChecker,
                             ISyncWaitChecker syncWaitChecker, ISyncWaitConverter syncWaitConverter)
        {
            this.asyncInvocationReplacer = asyncInvocationReplacer;
            this.invocationConverter = invocationConverter;
            this.syncWaitChecker = syncWaitChecker;
            this.awaitElider = awaitElider;
            this.awaitEliderChecker = awaitEliderChecker;
            this.syncWaitConverter = syncWaitConverter;
        }

        public void ReplaceToAsync(IMethod method)
        {
            foreach (var methodDeclaration in method
                .FindAllHierarchy()
                .SelectMany(x => x.GetDeclarations<IMethodDeclaration>()))
            {
                ReplaceMethodToAsync(methodDeclaration);
            }
        }

        private void ReplaceMethodToAsync(IMethodDeclaration method)
        {
            if (!method.IsValid())
                return;

            var methodDeclaredElement = method.DeclaredElement;
            if (methodDeclaredElement == null)
                return;

            var finder = method.GetPsiServices().Finder;
            var usages = finder.FindAllReferences(methodDeclaredElement);
            foreach (var usage in usages)
            {
                var invocation = usage.GetTreeNode().Parent as IInvocationExpression;
                asyncInvocationReplacer.ReplaceInvocation(invocation, GenerateAsyncMethodName(method.DeclaredName), invocation?.IsUnderAsyncDeclaration() ?? false);
            }

            //TODO: ugly hack. think
            IInvocationExpression invocationExpression;
            while ((invocationExpression
                       = method.DescendantsInScope<IInvocationExpression>().FirstOrDefault(syncWaitChecker.CanReplaceWaitToAsync)) != null)
                syncWaitConverter.ReplaceWaitToAsync(invocationExpression);

            IReferenceExpression referenceExpression;
            while ((referenceExpression
                       = method.DescendantsInScope<IReferenceExpression>().FirstOrDefault(syncWaitChecker.CanReplaceResultToAsync)) != null)
                syncWaitConverter.ReplaceResultToAsync(referenceExpression);

            var replace = true;
            while (replace)
            {
                replace = false;
                foreach (var invocationExpression2 in method.DescendantsInScope<IInvocationExpression>())
                {
                    if (invocationConverter.TryReplaceInvocationToAsync(invocationExpression2))
                    {
                        replace = true;
                        break;
                    }
                }
            }

            foreach (var parametersOwnerDeclaration in method
                .Descendants<IParametersOwnerDeclaration>()
                .ToEnumerable()
                .Where(awaitEliderChecker.CanElide))
            {
                awaitElider.Elide(parametersOwnerDeclaration);
            }

            ReplaceMethodSignatureToAsync(methodDeclaredElement, method);
        }

        private string GenerateAsyncMethodName([NotNull] string oldName) => oldName.EndsWith("Async") ? oldName : $"{oldName}Async";

        private void ReplaceMethodSignatureToAsync([NotNull] IParametersOwner parametersOwner, [NotNull] IMethodDeclaration methodDeclaration)
        {
            var returnType = parametersOwner.ReturnType;

            var psiModule = methodDeclaration.GetPsiModule();
            IDeclaredType newReturnValue;
            if (returnType.IsVoid())
            {
                newReturnValue = TypeFactory.CreateTypeByCLRName("System.Threading.Tasks.Task", psiModule);
            }
            else
            {
                var task = TypeFactory.CreateTypeByCLRName("System.Threading.Tasks.Task`1", psiModule).GetTypeElement();
                if (task == null)
                    return;
                newReturnValue = TypeFactory.CreateType(task, returnType);
            }

            var name = GenerateAsyncMethodName(methodDeclaration.DeclaredName);

            SetSignature(methodDeclaration, newReturnValue, name);

            if(awaitEliderChecker.CanElide(methodDeclaration))
                awaitElider.Elide(methodDeclaration);
        }

        private static void SetSignature([NotNull] IMethodDeclaration methodDeclaration, [NotNull] IType newReturnValue, [NotNull] string newName)
        {
            methodDeclaration.SetType(newReturnValue);
            methodDeclaration.SetAsync(methodDeclaration.Body != null);
            methodDeclaration.SetName(newName);
        }
    }
}