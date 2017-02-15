using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers
{
    public interface IParameterComparer
    {
        [Pure]
        bool IsParametersEqual([ItemNotNull] IList<IParameter> originalParameters, [ItemNotNull] IList<IParameter> methodParameters);
    }
}