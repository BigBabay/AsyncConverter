namespace AsyncConverter.AsyncHelpers.ParameterComparers
{
    public interface IParameterCompareResolver
    {
        ParameterCompareAggregateResult Resolve(CompareResult[] parameterResults);
    }
}