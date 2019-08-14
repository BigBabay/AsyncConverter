using AsyncConverter.Tests.Helpers;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.Highlightings.Naming
{
    [TestReferences("LibraryToOverride.dll")]
    public class NamingWithExternalAssamblyTests : HighlightingsTestsBase
    {
        protected sealed override string RelativeTestDataPath => @"Highlightings\Naming\WithOverride";

        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[] {@"Highlightings\Naming\WithOverride"})]
        public void Test(string fileName)
        {
            DoTestSolution(fileName);
        }
    }
}