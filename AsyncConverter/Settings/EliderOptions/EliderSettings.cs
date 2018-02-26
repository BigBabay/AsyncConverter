using JetBrains.Application.Settings;

namespace AsyncConverter.Settings.EliderOptions
{
    [SettingsKey(typeof(AsyncConverterSettings), "Settings for await elider")]
    public class EliderSettings
    {
        [SettingsEntry(true, "Do not suggest elide await in test method.")]
        public bool ExcludeTestMethodsFromAnalysis { get; set; }
    }
}