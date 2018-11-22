using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Controls.Dialogs;
using JetBrains.Application.UI.Controls.StringCollectionEdit.Impl;
using JetBrains.Application.UI.Controls.StringCollectionEdit.Impl.Buttons;
using JetBrains.Application.UI.Controls.StringCollectionEdit.Impl.Items;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;

namespace AsyncConverter.Settings.ConfigureAwaitOptions
{
    [OptionsPage(PID, "ConfigureAwait settings", typeof(ServicesThemedIcons.InspectionToolWindow), ParentId = AsyncConverterPage.PID)]
    public sealed class AsyncConverterConfigureAwaitPage : CustomSimpleOptionsPage
    {
        public const string PID = "AsyncConverterConfigureAwait";

        public AsyncConverterConfigureAwaitPage([NotNull] Lifetime lifetime, [NotNull] OptionsSettingsSmartContext store,
                                                IPromptWinForm promptWinForms)
            : base(lifetime, store)
        {
            AddBoolOption((AsyncConverterConfigureAwaitSettings options) => options.ExcludeTestMethodsFromConfigureAwait,
                "Do not suggest add 'ConfigureAwait' in test method.");

            AddHeader("In class and method under attributes will be ignored ConfigureAwait suggestion");
            var editItemViewModelFactory = new DefaultCollectionEditItemViewModelFactory(null);
            var buttonProviderFactory = new DefaultButtonProviderFactory(lifetime, promptWinForms, editItemViewModelFactory);
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