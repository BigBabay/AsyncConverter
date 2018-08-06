using System;
using System.Linq;
using AsyncConverter.AsyncHelpers.AwaitEliders;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeAnnotations;
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

        public AsyncReplacer(IAsyncInvocationReplacer asyncInvocationReplacer, IInvocationConverter invocationConverter, IAwaitElider awaitElider, IAwaitEliderChecker awaitEliderChecker)
        {
            this.asyncInvocationReplacer = asyncInvocationReplacer;
            this.invocationConverter = invocationConverter;
            this.awaitElider = awaitElider;
            this.awaitEliderChecker = awaitEliderChecker;
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
            while (true)
            {
                var allInvocationReplaced = method
                    .DescendantsInScope<IInvocationExpression>()
                    .All(invocationExpression => !invocationConverter.TryReplaceInvocationToAsync(invocationExpression));
                if(allInvocationReplaced)
                    break;
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
                ChangeJetbrainsAnnotations(methodDeclaration);
            }

            var name = GenerateAsyncMethodName(methodDeclaration.DeclaredName);

            SetSignature(methodDeclaration, newReturnValue, name);

            if(awaitEliderChecker.CanElide(methodDeclaration))
                awaitElider.Elide(methodDeclaration);
        }

        private static void ChangeJetbrainsAnnotations([NotNull] IMethodDeclaration methodDeclaration)
        {
            var itemCanBeNull = CodeAnnotationsUtil.GetNullabilityCache(methodDeclaration.UserData, methodDeclaration).ItemCanBeNullAnnotation;
            ChangeAnnotationAttribute(
                methodDeclaration,
                "CanBeNull",
                itemCanBeNull);

            var itemNotNull = CodeAnnotationsUtil.GetNullabilityCache(methodDeclaration.UserData, methodDeclaration).ItemNotNullAnnotation;
            ChangeAnnotationAttribute(
                methodDeclaration,
                "NotNull",
                itemNotNull);
        }

        private static void ChangeAnnotationAttribute(IMethodDeclaration methodDeclaration, string toReplace, ITypeElement replacement)
        {
            var toReplaceAttribute = methodDeclaration.Attributes
                .FirstOrDefault(a => string.Compare(a.Name.QualifiedName, toReplace, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (toReplaceAttribute != null && replacement != null)
            {
                var annotation = CSharpAnnotationsUtil.Annotate(methodDeclaration, replacement);
                if (annotation.Status == CSharpAnnotationsUtil.Status.AnnotationInserted)
                {
                    methodDeclaration.RemoveAttribute(toReplaceAttribute);
                }
            }
        }

        private static void SetSignature([NotNull] IMethodDeclaration methodDeclaration, [NotNull] IType newReturnValue, [NotNull] string newName)
        {
            methodDeclaration.SetType(newReturnValue);
            if(!methodDeclaration.IsAbstract)
                methodDeclaration.SetAsync(true);
            methodDeclaration.SetName(newName);
        }
    }
}