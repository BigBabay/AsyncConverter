using AsyncConverter.AsyncHelpers.ClassSearchers;
using AsyncConverter.AsyncHelpers.ParameterComparers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Util;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class AsyncMethodFinder : IAsyncMethodFinder
    {
        private readonly IClassForSearchResolver classForSearchResolver;
        private readonly IParameterComparer parameterComparer;

        public AsyncMethodFinder(IClassForSearchResolver classForSearchResolver, IParameterComparer parameterComparer)
        {
            this.classForSearchResolver = classForSearchResolver;
            this.parameterComparer = parameterComparer;
        }

        public FindingReslt FindEquivalentAsyncMethod(IParametersOwner originalMethod, IType invokedType)
        {
            if (!originalMethod.IsValid())
                return FindingReslt.CreateFail();

            var @class = classForSearchResolver.GetClassForSearch(originalMethod, invokedType);
            if (@class == null)
                return FindingReslt.CreateFail();

            var originalReturnType = originalMethod.Type();
            foreach (var candidateMethod in @class.Methods)
            {
                if (originalMethod.ShortName + "Async" != candidateMethod.ShortName)
                    continue;

                var returnType = candidateMethod.Type();
                if (!returnType.IsTaskOf(originalReturnType))
                    continue;

                var parameterCompareResult = parameterComparer.ComparerParameters(candidateMethod.Parameters, originalMethod.Parameters);
                if (!parameterCompareResult.CanBeConvertedToAsync())
                    continue;

                return new FindingReslt
                       {
                           Method = candidateMethod,
                           ParameterCompareResult = parameterCompareResult,
                       };
            }
            return FindingReslt.CreateFail();
        }
    }
}