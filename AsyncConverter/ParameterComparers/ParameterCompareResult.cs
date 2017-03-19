using JetBrains.Annotations;

namespace AsyncConverter.ParameterComparers
{
    public class ParameterCompareResult
    {
        public ParameterCompareAggregateResult Result { get; set; }
        [CanBeNull]
        public CompareResult[] ParameterResults { get; set; }
    }
}