using System.Collections.Generic;
using AsyncConverter.Helpers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers
{
    [SolutionComponent]
    public class ParameterComparer : IParameterComparer
    {
        public bool IsParametersEqual(IList<IParameter> originalParameters, IList<IParameter> methodParameters)
        {
            {
                if (methodParameters.Count != originalParameters.Count)
                    return false;

                for (var i = 0; i < methodParameters.Count; i++)
                {
                    var parameter = methodParameters[i];
                    var originalParameter = originalParameters[i];

                    if (!parameter.Type.IsEquals(originalParameter.Type))
                        return false;
                }
                return true;
            }
        }
    }
}