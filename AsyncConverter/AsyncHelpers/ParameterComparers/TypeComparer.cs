using AsyncConverter.Helpers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ParameterComparers
{
    [SolutionComponent]
    public class TypeComparer : ITypeComparer
    {
        public ParameterCompareResultAction Compare(IType originalParameterType, IType parameterType)
        {
            if (originalParameterType.IsEquals(parameterType))
                return ParameterCompareResultAction.Equal;
            if (originalParameterType.IsAsyncDelegate(parameterType))
                return ParameterCompareResultAction.NeedConvertToAsyncFunc;
            return ParameterCompareResultAction.NotEqual;
        }
    }
}