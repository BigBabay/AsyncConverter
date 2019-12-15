using JetBrains.Annotations;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.IDE.UI.Options;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.Resources;

namespace AsyncConverter.Settings.ConfigureAwaitOptions
{
    [OptionsPage(PID, "ConfigureAwait settings", typeof(ServicesThemedIcons.InspectionToolWindow), ParentId = AsyncConverterPage.PID)]
    public sealed class AsyncConverterConfigureAwaitPage : BeSimpleOptionsPage
    {
        public const string PID = "AsyncConverterConfigureAwait";

        public AsyncConverterConfigureAwaitPage(Lifetime lifetime, OptionsPageContext optionsPageContext, [NotNull] OptionsSettingsSmartContext store)
            : base(lifetime, optionsPageContext, store)
        {
            AddBoolOption((AsyncConverterConfigureAwaitSettings options) => options.ExcludeTestMethodsFromConfigureAwait,
                "Do not suggest add 'ConfigureAwait' in test method.");
        }
    }
}