using JetBrains.Application.Settings;

namespace AsyncConverter.Settings.ConfigureAwaitOptions
{
    [SettingsKey(typeof(AsyncConverterSettings), "Settings for ConfigureAwait")]
    public class AsyncConverterConfigureAwaitSettings
    {
        [SettingsEntry(true, "Do not suggest add 'ConfigureAwait' into test method.")]
        public bool ExcludeTestMethodsFromConfigureAwait { get; set; }

        [SettingsIndexedEntry("Custom attributes for ignoring ConfigureAwait.")]
        public IIndexedEntry<string, string> ConfigureAwaitIgnoreAttributeTypes { get; set; }
    }
}