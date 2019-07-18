using System;
using System.Linq;
using AsyncConverter.AsyncHelpers.MethodFinders;
using AsyncConverter.AsyncHelpers.ParameterComparers;
using AsyncConverter.Checkers.AsyncWait;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class InvocationConverter : IInvocationConverter
    {
        private readonly IAsyncMethodFinder asyncMethodFinder;
        private readonly IAsyncInvocationReplacer asyncInvocationReplacer;
        private readonly ISyncWaitChecker syncWaitChecker;
        private readonly ISyncWaitConverter syncWaitConverter;

        public InvocationConverter(
            IAsyncMethodFinder asyncMethodFinder,
            IAsyncInvocationReplacer asyncInvocationReplacer,
            ISyncWaitChecker syncWaitChecker,
            ISyncWaitConverter syncWaitConverter)
        {
            this.asyncMethodFinder = asyncMethodFinder;
            this.asyncInvocationReplacer = asyncInvocationReplacer;
            this.syncWaitChecker = syncWaitChecker;
            this.syncWaitConverter = syncWaitConverter;
        }

        public bool TryReplaceInvocationToAsync(IInvocationExpression invocationExpression)
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

                        IInvocationExpression innerInvocationExpression;
                        while ((innerInvocationExpression
                                   = lambdaExpression.DescendantsInScope<IInvocationExpression>().FirstOrDefault(syncWaitChecker.CanReplaceWaitToAsync)) != null)
                            syncWaitConverter.ReplaceWaitToAsync(innerInvocationExpression);

                        IReferenceExpression referenceExpression;
                        while ((referenceExpression
                                   = lambdaExpression.DescendantsInScope<IReferenceExpression>().FirstOrDefault(syncWaitChecker.CanReplaceResultToAsync)) != null)
                            syncWaitConverter.ReplaceResultToAsync(referenceExpression);

                        var innerInvocationExpressions = lambdaExpression.DescendantsInScope<IInvocationExpression>();
                        foreach (var innerInvocationExpression2 in innerInvocationExpressions)
                        {
                            if (!TryReplaceInvocationToAsync(innerInvocationExpression2))
                            {
                                //invocationExpression.PsiModule.GetPsiServices().Transactions.RollbackTransaction();
                            }
                        }
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