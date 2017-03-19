using AsyncConverter.AsyncHelpers.ParameterComparers;

namespace AsyncConverter.AsyncHelpers
{
    public interface IParameterCompareResolver
    {
        ParameterCompareAggregateResult Resolve(CompareResult[] parameterResults);
    }
}