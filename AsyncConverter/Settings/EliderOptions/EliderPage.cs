using AsyncConverter.Settings.AsyncSuffixOptions;
using JetBrains.Annotations;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;

namespace AsyncConverter.Settings.EliderOptions
{
    [OptionsPage(PID, "Await elider", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = AsyncConverterPage.PID)]
    public sealed class EliderPage : CustomSimpleOptionsPage
    {
        public const string PID = "Elider";

        public EliderPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, store)
        {
            AddBoolOption((AsyncConverterAsyncSuffixSettings options) => options.ExcludeTestMethodsFromAnalysis, "Exclude test methods from analysis");
        }
    }
}