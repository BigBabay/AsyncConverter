using AsyncConverter.Tests.Helpers;
using NUnit.Framework;

namespace AsyncConverter.Tests.Highlightings
{
    public class AsyncWaitTests : HighlightingsTestsBase
    {
        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[]{@"Highlightings\" + nameof(AsyncWaitTests)})]
        public void Test(string fileName)
        {
            DoTestSolution(fileName);
        }
    }
}