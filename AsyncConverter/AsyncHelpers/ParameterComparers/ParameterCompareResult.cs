using JetBrains.Annotations;

namespace AsyncConverter.AsyncHelpers.ParameterComparers
{
    public class ParameterCompareResult
    {
        public ParameterCompareAggregateResult Result { get; set; }
        [CanBeNull]
        public CompareResult[] ParameterResults { get; set; }
    }
}