using System;
using System.Linq.Expressions;
using AsyncConverter.Settings.AsyncSuffixOptions;
using AsyncConverter.Settings.ConfigureAwaitOptions;
using JetBrains.Annotations;
using JetBrains.Application.Settings.Store;

namespace AsyncConverter.Settings
{
    internal static class AsyncConverterSettingsAccessor
    {
        [NotNull]
        public static readonly Expression<Func<AsyncConverterNamingSettings, bool>>
            ExcludeTestMethodsFromAnalysis = x => x.ExcludeTestMethodsFromAnalysis;

        [NotNull]
        public static readonly Expression<Func<AsyncConverterConfigureAwaitSettings, IIndexedEntry<string, string>>>
            ConfigureAwaitIgnoreAttributeTypes =
                x => x.ConfigureAwaitIgnoreAttributeTypes;
    }
}