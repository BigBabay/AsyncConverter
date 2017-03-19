using AsyncConverter.AsyncHelpers;
using AsyncConverter.AsyncHelpers.ClassSearchers;
using AsyncConverter.ParameterComparers;
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

        public IMethod FindEquivalentAsyncMethod(IParametersOwner originalMethod, IType invokedType)
        {
            if (!originalMethod.IsValid())
                return null;

            var @class = classForSearchResolver.GetClassForSearch(originalMethod, invokedType);
            if (@class == null)
                return null;

            var originalReturnType = originalMethod.Type();
            foreach (var candidateMethod in @class.Methods)
            {
                if (originalMethod.ShortName + "Async" != candidateMethod.ShortName)
                    continue;

                var returnType = candidateMethod.Type();
                if (returnType.IsTask() && !originalReturnType.IsVoid())
                    continue;

                if (!returnType.IsGenericTaskOf(originalReturnType))
                    continue;

                var parameterCompareAggregateResult = parameterComparer.ComparerParameters(candidateMethod.Parameters, originalMethod.Parameters).Result;
                if(parameterCompareAggregateResult == ParameterCompareAggregateResult.NotEqual
                    || parameterCompareAggregateResult == ParameterCompareAggregateResult.DifferentLength)
                    continue;

                return candidateMethod;
            }
            return null;
        }
    }
}