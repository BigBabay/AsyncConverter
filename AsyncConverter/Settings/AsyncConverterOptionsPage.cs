using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Daemon.OptionPages;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionsDialog2.SimpleOptions;

namespace AsyncConverter.Settings
{
    [OptionsPage(pageId, "AsyncConverter", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = CodeInspectionPage.PID)]
    public sealed class AsyncConverterOptionsPage : CustomSimpleOptionsPage
    {
        private const string pageId = "AsyncConverter";

        public AsyncConverterOptionsPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, store)
        {
            AddHeader("Tests");
            AddBoolOption((AsyncConverterSettings options) => options.ExcludeTestMethodsFromAnalysis, "Exclude test methods from analysis");
        }
    }
}