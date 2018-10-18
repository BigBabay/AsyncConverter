using AsyncConverter.Settings.ConfigureAwaitOptions;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings.ConfigureAwait
{
    [TestSetting(typeof(AsyncConverterConfigureAwaitSettings), nameof(AsyncConverterConfigureAwaitSettings.ExcludeTestMethodsFromConfigureAwait), false)]
    public class ConfigureAwaitOnTestTests : HighlightingsTestsBase
    {
        protected override string Folder => "ConfigureAwait/OnTest";
    }
}