using System;
using AsyncConverter.AsyncHelpers.AwaitEliders;
using AsyncConverter.AsyncHelpers.MethodFinders;
using AsyncConverter.AsyncHelpers.ParameterComparers;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class InvocationConverter : IInvocationConverter
    {
        private readonly IAsyncInvocationReplacer asyncInvocationReplacer;
        private readonly IAwaitEliderChecker awaitEliderChecker;
        private readonly IAwaitElider awaitElider;
        private readonly IAsyncMethodFinder asyncMethodFinder;

        public InvocationConverter(
            IAsyncMethodFinder asyncMethodFinder,
            IAsyncInvocationReplacer asyncInvocationReplacer,
            IAwaitEliderChecker awaitEliderChecker,
            IAwaitElider awaitElider)
        {
            this.asyncMethodFinder = asyncMethodFinder;
            this.asyncInvocationReplacer = asyncInvocationReplacer;
            this.awaitEliderChecker = awaitEliderChecker;
            this.awaitElider = awaitElider;
        }

        public bool TryReplaceInvocationToAsync(IInvocationExpression invocationExpression)
        {
            if (invocationExpression.Type().IsGenericTask() || invocationExpression.Type().IsTask())
            {
                return TryConvertSyncToAwaitWaiting(invocationExpression);
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

        private bool TryConvertSyncToAwaitWaiting([NotNull] IInvocationExpression invocationExpression)
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
            try
            {
                for (var i = 0; i < arguments.Count; i++)
                {
                    var compareResult = parameterCompareResult.ParameterResults[i];
                    if (compareResult.Action == ParameterCompareResultAction.NeedConvertToAsyncFunc)
                    {
                        var lambdaExpression = arguments[i].Value as ILambdaExpression;
                        if (lambdaExpression == null)
                        {
                            invocationExpression.PsiModule.GetPsiServices().Transactions.RollbackTransaction();
                            return false;
                        }
                        lambdaExpression.SetAsync(true);
                        var innerInvocationExpressions = lambdaExpression.DescendantsInScope<IInvocationExpression>();
                        foreach (var innerInvocationExpression in innerInvocationExpressions)
                        {
                            if (!TryReplaceInvocationToAsync(innerInvocationExpression))
                            {
                                invocationExpression.PsiModule.GetPsiServices().Transactions.RollbackTransaction();
                                return false;
                            }
                        }

                        if (awaitEliderChecker.CanElide(lambdaExpression))
                            awaitElider.Elide(lambdaExpression);
                    }
                }
            }
            catch (Exception)
            {
                invocationExpression.PsiModule.GetPsiServices().Transactions.RollbackTransaction();
                return false;
            }

            invocationExpression.PsiModule.GetPsiServices().Transactions.CommitTransaction();
            return true;
        }
    }
}