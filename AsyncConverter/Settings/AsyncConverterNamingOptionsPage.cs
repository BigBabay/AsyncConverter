using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionsDialog2.SimpleOptions;

namespace AsyncConverter.Settings
{
    [OptionsPage(PID, "AsyncConverterNaming", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = AsyncConverterOptionsPage.PID)]
    public sealed class AsyncConverterNamingOptionsPage : CustomSimpleOptionsPage
    {
        public const string PID = "AsyncConverterNaming";

        public AsyncConverterNamingOptionsPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, store)
        {
            AddHeader("Async suffix");
            AddBoolOption((AsyncConverterNamingSettings options) => options.ExcludeTestMethodsFromAnalysis, "Exclude test methods from analysis");
        }
    }
}