using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Interop.NativeHook;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.UI.Application;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionsDialog2.SimpleOptions;
using JetBrains.UI.Validation;
using JetBrains.UI.Wpf.Controls.StringCollectionEdit.Impl;
using JetBrains.UI.Wpf.Controls.StringCollectionEdit.Impl.Buttons;
using JetBrains.UI.Wpf.Controls.StringCollectionEdit.Impl.Items;

namespace AsyncConverter.Settings.ConfigureAwaitOptions
{
    [OptionsPage(PID, "ConfigureAwait settings", typeof(ServicesThemedIcons.InspectionToolWindow), ParentId = AsyncConverterPage.PID)]
    public sealed class AsyncConverterConfigureAwaitPage : CustomSimpleOptionsPage
    {
        public const string PID = "AsyncConverterConfigureAwait";

        public AsyncConverterConfigureAwaitPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store,
            IWindowsHookManager windowsHookManager, FormValidators formValidators, IUIApplication iuiApplication)
            : base(lifetime, store)
        {
            AddHeader("In class and method will be ignored ConfigureAwait suggestion");
            var editItemViewModelFactory = new DefaultCollectionEditItemViewModelFactory(null);
            var buttonProviderFactory = new DefaultButtonProviderFactory(lifetime, windowsHookManager, formValidators,
                iuiApplication, editItemViewModelFactory);
            var attributeTypes = new StringCollectionEditViewModel(lifetime, "Attributes:",
                buttonProviderFactory, editItemViewModelFactory);
            foreach (var type in store.EnumIndexedValues(AsyncConverterSettingsAccessor.ConfigureAwaitIgnoreAttributeTypes))
            {
                attributeTypes.AddItem(type);
            }

            attributeTypes.Items.CollectionChanged += (o, e) =>
                                                        {
                                                            foreach (
                                                                var entryIndex in
                                                                OptionsSettingsSmartContext.EnumEntryIndices(AsyncConverterSettingsAccessor.ConfigureAwaitIgnoreAttributeTypes)
                                                                    .ToArray())
                                                            {
                                                                OptionsSettingsSmartContext.RemoveIndexedValue(AsyncConverterSettingsAccessor.ConfigureAwaitIgnoreAttributeTypes,
                                                                    entryIndex);
                                                            }
                                                            foreach (
                                                                var editItemViewModel in
                                                                attributeTypes.Items)
                                                            {
                                                                OptionsSettingsSmartContext.SetIndexedValue(AsyncConverterSettingsAccessor.ConfigureAwaitIgnoreAttributeTypes,
                                                                    editItemViewModel.PresentableName, editItemViewModel.PresentableName);
                                                            }
                                                        };
            AddCustomOption(attributeTypes);

        }
    }
}