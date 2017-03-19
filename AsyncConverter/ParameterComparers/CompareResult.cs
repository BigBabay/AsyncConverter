using JetBrains.ReSharper.Psi;

namespace AsyncConverter.ParameterComparers
{
    public class CompareResult
    {
        public IType From { get; set; }
        public IType To { get; set; }
        public ParameterCompareResultAction Action { get; set; }
    }
}