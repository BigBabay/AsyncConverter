using JetBrains.Application.Settings;

namespace AsyncConverter.Settings
{
    [SettingsKey(typeof(AsyncConverterAsyncSuffixSettings), "Settings for 'Async' suffix")]
    public class AsyncConverterAsyncSuffixSettings
    {
        [SettingsEntry(true, "Suggest adding 'Async' suffix to test method.")]
        public bool ExcludeTestMethodsFromAnalysis { get; set; }
    }
}