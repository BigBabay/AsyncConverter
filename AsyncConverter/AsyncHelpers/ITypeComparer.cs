using AsyncConverter.AsyncHelpers.ParameterComparers;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers
{
    public interface ITypeComparer
    {
        ParameterCompareResultAction Compare(IType originalParameterType, IType parameterType);
    }
}