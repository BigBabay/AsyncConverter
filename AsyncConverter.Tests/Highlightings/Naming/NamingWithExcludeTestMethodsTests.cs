using AsyncConverter.Settings.General;
using AsyncConverter.Tests.Helpers;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.Highlightings.Naming
{
    [TestSetting(typeof(GeneralSettings), nameof(GeneralSettings.ExcludeTestMethodsFromRanaming), false)]
    public class NamingWithExcludeTestMethodsTests : HighlightingsTestsBase
    {
        protected sealed override string RelativeTestDataPath => @"Highlightings\Naming\WithExcludeTest";

        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[]{@"Highlightings\Naming\WithExcludeTest"})]
        public void Test(string fileName)
        {
            DoTestSolution(fileName);
        }
    }
}