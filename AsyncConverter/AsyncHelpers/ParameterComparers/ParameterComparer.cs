using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ParameterComparers
{
    [SolutionComponent]
    internal class ParameterComparer : IParameterComparer
    {
        private readonly ITypeComparer typeComparer;

        public ParameterComparer(ITypeComparer typeComparer)
        {
            this.typeComparer = typeComparer;
        }

        public ParameterCompareResult ComparerParameters(IList<IParameter> originalParameters, IList<IParameter> methodParameters)
        {
            if (methodParameters.Count != originalParameters.Count)
                return ParameterCompareResult.CreateFailDifferentLength();

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
            return ParameterCompareResult.Create(parameterResults);
        }
    }
}