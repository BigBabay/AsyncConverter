using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ParameterComparers
{
    public interface ITypeComparer
    {
        ParameterCompareResultAction Compare(IType originalParameterType, IType parameterType);
    }
}