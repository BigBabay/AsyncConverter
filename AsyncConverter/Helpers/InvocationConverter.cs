using AsyncConverter.AsyncHelpers.ParameterComparers;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class InvocationConverter : IInvocationConverter
    {
        private readonly IAsyncInvocationReplacer asyncInvocationReplacer;
        private readonly IAsyncMethodFinder asyncMethodFinder;

        public InvocationConverter(IAsyncMethodFinder asyncMethodFinder, IAsyncInvocationReplacer asyncInvocationReplacer)
        {
            this.asyncMethodFinder = asyncMethodFinder;
            this.asyncInvocationReplacer = asyncInvocationReplacer;
        }

        public bool TryReplaceInvocationToAsync(IInvocationExpression invocationExpression)
        {
            if (invocationExpression.Type().IsGenericTask() || invocationExpression.Type().IsTask())
            {
                return TryConvertCallWithWaitToAwaitCall(invocationExpression);
            }

            return TryConvertSyncCallToAsyncCall(invocationExpression);
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

            var findingResult = asyncMethodFinder.FindEquivalentAsyncMethod(invocationMethod, invokedType);
            if (findingResult.CanBeConvertedToAsync())
            {
                if (!TryConvertParameterFuncToAsync(invocationExpression, findingResult.ParameterCompareResult))
                    return false;

                asyncInvocationReplacer.ReplaceInvocation(invocationExpression, findingResult.Method.ShortName, true);
                return true;
            }
            return false;
        }

        private bool TryConvertCallWithWaitToAwaitCall([NotNull] IInvocationExpression invocationExpression)
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
    }
}