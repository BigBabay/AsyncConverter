using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionsDialog2.SimpleOptions;

namespace AsyncConverter.Settings
{
    [OptionsPage(PID, "AsyncConverterConfigureAwait", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = AsyncConverterOptionsPage.PID)]
    public sealed class AsyncConverterConfigureAwaitOptionsPage : CustomSimpleOptionsPage
    {
        public const string PID = "AsyncConverterConfigureAwait";

        public AsyncConverterConfigureAwaitOptionsPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, store)
        {
            AddHeader("Configure await settings");
            AddBoolOption((AsyncConverterNamingSettings options) => options.ExcludeTestMethodsFromAnalysis, "Exclude test methods from analysis");
        }
    }
}