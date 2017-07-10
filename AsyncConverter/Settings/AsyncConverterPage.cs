using JetBrains.ReSharper.Feature.Services.Daemon.OptionPages;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.UI.Options;
using JetBrains.UI.Options.Helpers;

namespace AsyncConverter.Settings
{
    [OptionsPage(PID, "Async Converter", typeof(ServicesThemedIcons.FileStorage), ParentId = CodeInspectionPage.PID)]
    public sealed class AsyncConverterPage : AEmptyOptionsPage
    {
        public const string PID = "AsyncConverter";
    }
}