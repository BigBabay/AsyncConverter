using AsyncConverter.Tests.Helpers;
using NUnit.Framework;

namespace AsyncConverter.Tests.Highlightings.Eliding
{
    public class ElidingTests : HighlightingsTestsBase
    {
        protected sealed override string RelativeTestDataPath => @"Highlightings\Eliding\Common";

        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[] {@"Highlightings\Eliding\Common"})]
        public void Test(string fileName)
        {
            DoTestSolution(fileName);
        }
    }
}