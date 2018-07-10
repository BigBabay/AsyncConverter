using AsyncConverter.Settings.General;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings
{
    [TestSetting(typeof(GeneralSettings), nameof(GeneralSettings.ExcludeTestMethodsFromConfigureAwait), false)]
    public class ConfigureAwaitOnTestTests : HighlightingsTestsBase
    {
        protected override string Folder => "ConfigureAwait/OnTest";
    }
}