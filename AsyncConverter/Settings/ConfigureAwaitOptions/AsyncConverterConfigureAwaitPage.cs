using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionsDialog2.SimpleOptions;

namespace AsyncConverter.Settings.ConfigureAwaitOptions
{
    [OptionsPage(PID, "ConfigureAwait settings", typeof(ServicesThemedIcons.InspectionToolWindow), ParentId = AsyncConverterPage.PID)]
    public sealed class AsyncConverterConfigureAwaitPage : CustomSimpleOptionsPage
    {
        public const string PID = "AsyncConverterConfigureAwait";

        public AsyncConverterConfigureAwaitPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, store)
        {
        }
    }
}