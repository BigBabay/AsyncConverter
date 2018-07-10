using AsyncConverter.Settings.ConfigureAwaitOptions;
using JetBrains.Application.Settings;

namespace AsyncConverter.Tests.Highlightings
{
    public class ConfigureAwaitWithAttributeTests : HighlightingsTestsBase
    {
        protected override void MutateSettings(IContextBoundSettingsStore settingsStore)
        {
            settingsStore.SetIndexedValue((AsyncConverterConfigureAwaitSettings s) => s.ConfigureAwaitIgnoreAttributeTypes, "MyCustomAttribute", "AsyncConverter.Tests.Test.Data.Highlightings.ConfigureAwaitWithAttribute.MyCustomAttribute");
        }
        protected override string Folder => "ConfigureAwait/WithAttribute";
    }
}