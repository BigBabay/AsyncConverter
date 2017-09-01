using JetBrains.Annotations;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;

namespace AsyncConverter.Settings.AsyncSuffixOptions
{
    [OptionsPage(PID, "Async suffix", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = AsyncConverterPage.PID)]
    public sealed class AsyncConverterAsyncSuffixPage : CustomSimpleOptionsPage
    {
        public const string PID = "AsyncConverterNaming";

        public AsyncConverterAsyncSuffixPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, store)
        {
            AddBoolOption((AsyncConverterAsyncSuffixSettings options) => options.ExcludeTestMethodsFromAnalysis, "Exclude test methods from analysis");
        }
    }
}