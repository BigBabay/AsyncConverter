using AsyncConverter.Helpers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ParameterComparers
{
    [SolutionComponent]
    internal class TypeComparer : ITypeComparer
    {
        public ParameterCompareResultAction Compare(IType originalParameterType, IType parameterType)
        {
            if (parameterType.IsEquals(originalParameterType))
                return ParameterCompareResultAction.Equal;
            if (parameterType.IsAsyncDelegate(originalParameterType))
                return ParameterCompareResultAction.NeedConvertToAsyncFunc;
            return ParameterCompareResultAction.NotEqual;
        }
    }
}