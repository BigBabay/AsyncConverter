using AsyncConverter.ParameterComparers;

namespace AsyncConverter.AsyncHelpers
{
    public interface IParameterCompareResolver
    {
        ParameterCompareAggregateResult Resolve(CompareResult[] parameterResults);
    }
}