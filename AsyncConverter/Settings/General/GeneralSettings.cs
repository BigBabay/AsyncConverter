using JetBrains.Application.Settings;

namespace AsyncConverter.Settings.General
{
    [SettingsKey(typeof(AsyncConverterSettings), "Settings for AsyncConverter plugin.")]
    public class GeneralSettings
    {
        [SettingsEntry(true, "Do not suggest elide await in test method.")]
        public bool ExcludeTestMethodsFromEliding{ get; set; }

        [SettingsEntry(true, "Do not suggest add 'Async' suffix to test method.")]
        public bool ExcludeTestMethodsFromRanaming { get; set; }

    }
}