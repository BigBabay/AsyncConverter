using JetBrains.Application.Settings;

namespace AsyncConverter.Settings.ConfigureAwaitOptions
{
    [SettingsKey(typeof(AsyncConverterSettings), "Settings for ConfigureAwait")]
    public class AsyncConverterConfigureAwaitSettings
    {
        [SettingsIndexedEntry("Custom attributes for ignoring ConfigureAwait.")]
        public IIndexedEntry<string, string> ConfigureAwaitIgnoreAttributeTypes { get; set; }
    }
}