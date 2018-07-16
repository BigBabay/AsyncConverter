using AsyncConverter.Settings.ConfigureAwaitOptions;
using AsyncConverter.Settings.General;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings.ConfigureAwait
{
    [TestSetting(typeof(GeneralSettings), nameof(AsyncConverterConfigureAwaitSettings.ExcludeTestMethodsFromConfigureAwait), false)]
    public class ConfigureAwaitOnTestTests : HighlightingsTestsBase
    {
        protected override string Folder => "ConfigureAwait/OnTest";
    }
}