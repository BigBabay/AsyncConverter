using System;
using System.Linq.Expressions;
using AsyncConverter.Settings.ConfigureAwaitOptions;
using AsyncConverter.Settings.General;
using JetBrains.Annotations;
using JetBrains.Application.Settings;

namespace AsyncConverter.Settings
{
    public static class AsyncConverterSettingsAccessor
    {
        [NotNull]
        public static readonly Expression<Func<GeneralSettings, bool>>
            ExcludeTestMethodsFromRenaming = x => x.ExcludeTestMethodsFromRanaming;

        [NotNull]
        public static readonly Expression<Func<GeneralSettings, bool>>
            ExcludeTestMethodsFromEliding = x => x.ExcludeTestMethodsFromEliding;

        [NotNull]
        public static readonly Expression<Func<AsyncConverterConfigureAwaitSettings, bool>>
            ExcludeTestMethodsFromConfigureAwait = x => x.ExcludeTestMethodsFromConfigureAwait;

        [NotNull]
        public static readonly Expression<Func<AsyncConverterConfigureAwaitSettings, IIndexedEntry<string, string>>>
            ConfigureAwaitIgnoreAttributeTypes =
                x => x.ConfigureAwaitIgnoreAttributeTypes;
    }
}