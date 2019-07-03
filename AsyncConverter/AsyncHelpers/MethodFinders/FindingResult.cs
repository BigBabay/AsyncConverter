using AsyncConverter.AsyncHelpers.ParameterComparers;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    public class FindingResult
    {
        public IMethod Method { get; set; }
        public ParameterCompareResult ParameterCompareResult { get; set; }

        public static FindingResult CreateFail() => new FindingResult();

        public bool CanBeConvertedToAsync() => Method != null && ParameterCompareResult.CanBeConvertedToAsync();
    }
}