using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ParameterComparers
{
    public interface IParameterComparer
    {
        [Pure]
        [NotNull]
        ParameterCompareResult ComparerParameters([ItemNotNull] IList<IParameter> originalParameters, [ItemNotNull] IList<IParameter> methodParameters);
    }
}