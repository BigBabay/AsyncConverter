using JetBrains.Application.Settings;

namespace AsyncConverter.Settings
{
    [SettingsKey(typeof(AsyncConverterSettings), "Settings for 'Async' suffix")]
    public class AsyncConverterNamingSettings
    {
        [SettingsEntry(true, "Suggest adding 'Async' suffix to test method.")]
        public bool ExcludeTestMethodsFromAnalysis { get; set; }
    }
}