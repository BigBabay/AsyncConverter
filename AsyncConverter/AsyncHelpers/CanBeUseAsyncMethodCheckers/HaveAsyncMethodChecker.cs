using AsyncConverter.AsyncHelpers.MethodFinders;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.CanBeUseAsyncMethodCheckers
{
    [SolutionComponent]
    internal class HaveAsyncMethodChecker : IConcreteCanBeUseAsyncMethodChecker
    {
        private readonly IAsyncMethodFinder asyncMethodFinder;

        public HaveAsyncMethodChecker(IAsyncMethodFinder asyncMethodFinder)
        {
            this.asyncMethodFinder = asyncMethodFinder;
        }

        public bool CanReplace(IInvocationExpression element)
        {
            var referenceCurrentResolveResult = element.Reference?.Resolve();
            if (referenceCurrentResolveResult?.IsValid() != true)
                return false;

            var invocationMethod = referenceCurrentResolveResult.DeclaredElement as IMethod;
            if (invocationMethod == null)
                return false;

            var invokedType = (element.ConditionalQualifier as IReferenceExpression)?.QualifierExpression?.Type();

            var findingResult = asyncMethodFinder.FindEquivalentAsyncMethod(invocationMethod, invokedType);
            if (findingResult.Method == null || !findingResult.ParameterCompareResult.CanBeConvertedToAsync())
                return false;

            return true;
        }
    }
}