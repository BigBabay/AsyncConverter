using JetBrains.Annotations;
using JetBrains.Application.Interop.NativeHook;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Daemon.OptionPages;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.UI.Application;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionsDialog2.SimpleOptions;
using JetBrains.UI.Validation;

namespace AsyncConverter.Settings
{
    [OptionsPage(pageId, "AsyncSuffix", typeof(ServicesThemedIcons.AnalyzeThis), ParentId = CodeInspectionPage.PID)]
    public sealed class AsyncConverterOptionsPage : CustomSimpleOptionsPage
    {
        private const string pageId = "AsyncConverter";

        public AsyncConverterOptionsPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store,
                                         IWindowsHookManager windowsHookManager, FormValidators formValidators, IUIApplication iuiApplication)
            : base(lifetime, store)
        {
            AddHeader("Tests");
            AddBoolOption((AsyncConverterSettings options) => options.ExcludeTestMethodsFromAnalysis, "Exclude test methods from analysis");
        }
    }
}