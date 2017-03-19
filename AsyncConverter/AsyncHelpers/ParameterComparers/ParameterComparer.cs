using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ParameterComparers
{
    [SolutionComponent]
    public class ParameterComparer : IParameterComparer
    {
        private readonly ITypeComparer typeComparer;
        private readonly IParameterCompareResolver parameterCompareResolver;

        public ParameterComparer(ITypeComparer typeComparer, IParameterCompareResolver parameterCompareResolver)
        {
            this.typeComparer = typeComparer;
            this.parameterCompareResolver = parameterCompareResolver;
        }

        public ParameterCompareResult ComparerParameters(IList<IParameter> originalParameters, IList<IParameter> methodParameters)
        {
            if (methodParameters.Count != originalParameters.Count)
                return new ParameterCompareResult {Result = ParameterCompareAggregateResult.DifferentLength};

            var parameterResults = new CompareResult[methodParameters.Count];
            for (var i = 0; i < methodParameters.Count; i++)
            {
                var parameter = methodParameters[i];
                var originalParameter = originalParameters[i];

                parameterResults[i] = new CompareResult
                                      {
                                          From = originalParameter.Type,
                                          To = parameter.Type,
                                          Action = typeComparer.Compare(originalParameter.Type, parameter.Type),
                                      };
            }
            return new ParameterCompareResult {Result = parameterCompareResolver.Resolve(parameterResults), ParameterResults = parameterResults};
        }
    }
}