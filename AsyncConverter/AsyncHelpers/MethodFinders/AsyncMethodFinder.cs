using AsyncConverter.AsyncHelpers.ClassSearchers;
using AsyncConverter.AsyncHelpers.ParameterComparers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    [SolutionComponent]
    public class AsyncMethodFinder : IAsyncMethodFinder
    {
        private readonly IClassForSearchResolver classForSearchResolver;
        private readonly IParameterComparer parameterComparer;
        private readonly IMethodFindingChecker methodFindingChecker;

        public AsyncMethodFinder(IClassForSearchResolver classForSearchResolver, IParameterComparer parameterComparer, IMethodFindingChecker methodFindingChecker)
        {
            this.classForSearchResolver = classForSearchResolver;
            this.parameterComparer = parameterComparer;
            this.methodFindingChecker = methodFindingChecker;
        }

        public FindingResult FindEquivalentAsyncMethod(IMethod originalMethod, IType invokedType)
        {
            if (!originalMethod.IsValid())
                return FindingResult.CreateFail();

            var @class = classForSearchResolver.GetClassForSearch(originalMethod, invokedType);
            if (@class == null)
                return FindingResult.CreateFail();

            foreach (var candidateMethod in @class.Methods)
            {
                if (methodFindingChecker.NeedSkip(originalMethod, candidateMethod))
                {
                    continue;
                }

                var parameterCompareResult = parameterComparer.ComparerParameters(candidateMethod.Parameters, originalMethod.Parameters);
                if (!parameterCompareResult.CanBeConvertedToAsync())
                    continue;

                return new FindingResult
                       {
                           Method = candidateMethod,
                           ParameterCompareResult = parameterCompareResult,
                       };
            }
            return FindingResult.CreateFail();
        }
    }
}