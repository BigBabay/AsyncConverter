using JetBrains.Annotations;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.DataFlow;
using JetBrains.IDE.UI.Options;
using JetBrains.ReSharper.Feature.Services.Resources;

namespace AsyncConverter.Settings.General
{
    [OptionsPage(PID, "General", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = AsyncConverterPage.PID)]
    public sealed class GeneralPage : BeSimpleOptionsPage
    {
        public const string PID = "General";

        public GeneralPage([NotNull] Lifetime lifetime, [NotNull] OptionsPageContext optionsPageContext, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, optionsPageContext, store)
        {
            AddHeader("Naming options");
            AddBoolOption((GeneralSettings options) => options.ExcludeTestMethodsFromRanaming, "Do not suggest add 'Async' suffix to test method.");

            AddHeader("Eliding options");
            AddBoolOption((GeneralSettings options) => options.ExcludeTestMethodsFromEliding, "Do not suggest elide await in test method.");
        }
    }
}