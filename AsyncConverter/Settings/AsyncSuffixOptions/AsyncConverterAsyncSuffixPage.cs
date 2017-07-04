using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionsDialog2.SimpleOptions;

namespace AsyncConverter.Settings.AsyncSuffixOptions
{
    [OptionsPage(PID, "AsyncConverterNaming", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = AsyncConverterPage.PID)]
    public sealed class AsyncConverterAsyncSuffixPage : CustomSimpleOptionsPage
    {
        public const string PID = "AsyncConverterNaming";

        public AsyncConverterAsyncSuffixPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, store)
        {
            AddHeader("Async suffix");
            AddBoolOption((AsyncConverterAsyncSuffixSettings options) => options.ExcludeTestMethodsFromAnalysis, "Exclude test methods from analysis");
        }
    }
}