using AsyncConverter.Settings.ConfigureAwaitOptions;
using AsyncConverter.Tests.Helpers;
using JetBrains.Application.Settings;
using NUnit.Framework;

namespace AsyncConverter.Tests.Highlightings.ConfigureAwait
{
    public class ConfigureAwaitWithAttributeTests : HighlightingsTestsBase
    {
        protected sealed override string RelativeTestDataPath => @"Highlightings\ConfigureAwait\WithAttribute";

        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[] {@"Highlightings\ConfigureAwait\WithAttribute"})]
        public void Test(string fileName)
        {
            DoTestSolution(fileName);
        }

        protected override void MutateSettings(IContextBoundSettingsStore settingsStore)
        {
            settingsStore.SetIndexedValue((AsyncConverterConfigureAwaitSettings s) => s.ConfigureAwaitIgnoreAttributeTypes, "MyCustomAttribute", "AsyncConverter.Tests.Test.Data.Highlightings.ConfigureAwaitWithAttribute.MyCustomAttribute");
        }
    }
}