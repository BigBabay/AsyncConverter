using AsyncConverter.Settings.ConfigureAwaitOptions;
using AsyncConverter.Tests.Helpers;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.Highlightings.ConfigureAwait
{
    [TestSetting(typeof(AsyncConverterConfigureAwaitSettings), nameof(AsyncConverterConfigureAwaitSettings.ExcludeTestMethodsFromConfigureAwait), false)]
    public class ConfigureAwaitOnTestTests : HighlightingsTestsBase
    {
        protected sealed override string RelativeTestDataPath => @"Highlightings\ConfigureAwait\OnTest";

        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[] {@"Highlightings\ConfigureAwait\OnTest"})]
        public void Test(string fileName)
        {
            DoTestSolution(fileName);
        }
    }
}