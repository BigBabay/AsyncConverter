using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace AsyncConverter.Settings
{
    [SettingsKey(typeof(CodeInspectionSettings), "Settings for AsyncConverter plugin.")]
    public class AsyncConverterSettings
    {
        [SettingsEntry(false, "Suggest adding 'Async' suffix to test method.")]
        public bool ExcludeTestMethodsFromAnalysis { get; set; }
    }
}
