using JetBrains.Annotations;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;

namespace AsyncConverter.Settings.General
{
    [OptionsPage(PID, "General", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = AsyncConverterPage.PID)]
    public sealed class GeneralPage : CustomSimpleOptionsPage
    {
        public const string PID = "General";

        public GeneralPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, store)
        {
            AddHeader("Naming options");
            AddBoolOption((GeneralSettings options) => options.ExcludeTestMethodsFromRanaming, "Do not suggest elide await in test method.");

            AddHeader("Eliding options");
            AddBoolOption((GeneralSettings options) => options.ExcludeTestMethodsFromEliding, "Do not suggest adding 'Async' suffix to test method.");
        }
    }
}