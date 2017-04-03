using System.Linq;
using AsyncConverter.AsyncHelpers.ParameterComparers;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class AsyncReplacer : IAsyncReplacer
    {
        private readonly IAsyncInvocationReplacer asyncInvocationReplacer;
        private readonly IAsyncMethodFinder asyncMethodFinder;

        public AsyncReplacer(IAsyncInvocationReplacer asyncInvocationReplacer, IAsyncMethodFinder asyncMethodFinder)
        {
            this.asyncInvocationReplacer = asyncInvocationReplacer;
            this.asyncMethodFinder = asyncMethodFinder;
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
            while (true)
            {
                var allInvocationReplaced = method
                    .Body
                    .Descendants<IInvocationExpression>()
                    .ToEnumerable()
                    .All(invocationExpression => !TryReplaceInvocationToAsync(invocationExpression));
                if(allInvocationReplaced)
                    break;
            }

            ReplaceMethodSignatureToAsync(methodDeclaredElement, method);
        }

        private string GenerateAsyncMethodName([NotNull] string oldName)
        {
            return oldName.EndsWith("Async") ? oldName : $"{oldName}Async";
        }

        private bool TryConvertSyncCallToAsyncCall([NotNull] IInvocationExpression invocationExpression)
        {
            var referenceCurrentResolveResult = invocationExpression.Reference?.Resolve();
            if (referenceCurrentResolveResult?.IsValid() != true)
                return false;

            var invocationMethod = referenceCurrentResolveResult.DeclaredElement as IMethod;
            if (invocationMethod == null)
                return false;

            var invokedType = (invocationExpression.ConditionalQualifier as IReferenceExpression)?.QualifierExpression?.Type();

            var findingReslt = asyncMethodFinder.FindEquivalentAsyncMethod(invocationMethod, invokedType);
            if (findingReslt.CanBeConvertedToAsync())
            {
                if (!TryConvertParameterFuncToAsync(invocationExpression, findingReslt.ParameterCompareResult))
                    return false;

                asyncInvocationReplacer.ReplaceInvocation(invocationExpression, GenerateAsyncMethodName(findingReslt.Method.ShortName), true);
                return true;
            }
            return false;
        }

        public bool TryReplaceInvocationToAsync(IInvocationExpression invocationExpression)
        {
            if (invocationExpression.Type().IsGenericTask() || invocationExpression.Type().IsTask())
            {
                return ConvertCallWithWaitToAwaitCall(invocationExpression);
            }

            return TryConvertSyncCallToAsyncCall(invocationExpression);
        }

        private bool TryConvertInnerReferenceToAsync([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory)
        {
            var referenceExpression = invocationExpression.FirstChild as IReferenceExpression;
            if (referenceExpression == null)
                return false;

            foreach (var child in referenceExpression.Children())
            {
                var invocationChild = child as IInvocationExpression;
                if (invocationChild != null)
                    TryReplaceInvocationToAsync(invocationChild);
            }
            return true;
        }

        private bool TryConvertParameterFuncToAsync([NotNull] IInvocationExpression invocationExpression, [NotNull] ParameterCompareResult parameterCompareResult)
        {
            var arguments = invocationExpression.Arguments;
            invocationExpression.PsiModule.GetPsiServices().Transactions.StartTransaction("convertAsyncParameter");
            for (var i = 0; i < arguments.Count; i++)
            {
                var compareResult = parameterCompareResult.ParameterResults[i];
                if (compareResult.Action == ParameterCompareResultAction.NeedConvertToAsyncFunc)
                {
                    var lambdaExpression = arguments[i].Value as ILambdaExpression;
                    if(lambdaExpression == null)
                    {
                        invocationExpression.PsiModule.GetPsiServices().Transactions.RollbackTransaction();
                        return false;
                    }
                    lambdaExpression.SetAsync(true);
                    var innerInvocationExpressions = lambdaExpression.Descendants<IInvocationExpression>();
                    foreach (var innerInvocationExpression in innerInvocationExpressions)
                    {
                        if (!TryReplaceInvocationToAsync(innerInvocationExpression))
                        {
                            invocationExpression.PsiModule.GetPsiServices().Transactions.RollbackTransaction();
                            return false;
                        }
                    }
                }
            }
            invocationExpression.PsiModule.GetPsiServices().Transactions.CommitTransaction();
            return true;
        }

        private bool ConvertCallWithWaitToAwaitCall([NotNull] IInvocationExpression invocationExpression)
        {
            var reference = invocationExpression.Parent as IReferenceExpression;

            var factory = CSharpElementFactory.GetInstance(invocationExpression);

            //TODO: AwaitResult our custom method extension, it must be moved to settings
            if (reference?.NameIdentifier?.Name == "AwaitResult" || reference?.NameIdentifier?.Name == "Wait")
            {
                var call = factory.CreateExpression("await $0($1).ConfigureAwait(false)", invocationExpression.ConditionalQualifier,
                    invocationExpression.ArgumentList);
                var parentInvocation = reference.Parent as IInvocationExpression;
                if (parentInvocation == null)
                    return false;
                parentInvocation.ReplaceBy(call);
                return true;
            }

            if (reference?.NameIdentifier?.Name == "Result")
            {
                var call = factory.CreateExpression("await $0($1).ConfigureAwait(false)", invocationExpression.ConditionalQualifier,
                    invocationExpression.ArgumentList);
                reference.ReplaceBy(call);
                return true;
            }
            return false;
        }

        private void ReplaceMethodSignatureToAsync([NotNull] IParametersOwner methodDeclaredElement, [NotNull] IMethodDeclaration methodDeclaration)
        {
            var returnType = methodDeclaredElement.ReturnType;

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
        }

        private static void SetSignature([NotNull] IMethodDeclaration methodDeclaration, [NotNull] IType newReturnValue, [NotNull] string newName)
        {
            methodDeclaration.SetType(newReturnValue);
            if (!methodDeclaration.IsAbstract)
                methodDeclaration.SetAsync(true);
            methodDeclaration.SetName(newName);
        }
    }
}