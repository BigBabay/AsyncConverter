using AsyncConverter.AsyncHelpers.ParameterComparers;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.Helpers
{
    public class FindingReslt
    {
        public IMethod Method { get; set; }
        public ParameterCompareResult ParameterCompareResult { get; set; }

        public static FindingReslt CreateFail() => new FindingReslt();

        public bool CanBeConvertedToAsync() => Method != null && ParameterCompareResult.CanBeConvertedToAsync();
    }
}