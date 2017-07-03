using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionsDialog2.SimpleOptions;

namespace AsyncConverter.Settings
{
    [OptionsPage(PID, "AsyncConverterAsyncSuffix", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = AsyncConverterOptionsPage.PID)]
    public sealed class AsyncConverterAsyncSuffixOptionsPage : CustomSimpleOptionsPage
    {
        public const string PID = "AsyncConverterAsyncSuffix";

        public AsyncConverterAsyncSuffixOptionsPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, store)
        {
            AddHeader("Tests");
            AddBoolOption((AsyncConverterAsyncSuffixSettings options) => options.ExcludeTestMethodsFromAnalysis, "Exclude test methods from analysis");
        }
    }
}