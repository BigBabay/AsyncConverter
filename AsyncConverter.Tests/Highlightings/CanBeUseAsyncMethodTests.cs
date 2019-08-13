using AsyncConverter.Tests.Helpers;
using NUnit.Framework;

namespace AsyncConverter.Tests.Highlightings
{
    public class CanBeUseAsyncMethodTests : HighlightingsTestsBase
    {
        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[]{@"Highlightings\" + nameof(CanBeUseAsyncMethodTests)})]
        public void Test(string fileName)
        {
            DoTestSolution(fileName);
        }
    }
}