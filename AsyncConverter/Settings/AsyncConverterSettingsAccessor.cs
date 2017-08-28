using System;
using System.Linq.Expressions;
using AsyncConverter.Settings.AsyncSuffixOptions;
using AsyncConverter.Settings.ConfigureAwaitOptions;
using JetBrains.Annotations;
using JetBrains.Application.Settings;

namespace AsyncConverter.Settings
{
    public static class AsyncConverterSettingsAccessor
    {
        [NotNull]
        public static readonly Expression<Func<AsyncConverterAsyncSuffixSettings, bool>>
            ExcludeTestMethodsFromAnalysis = x => x.ExcludeTestMethodsFromAnalysis;

        [NotNull]
        public static readonly Expression<Func<AsyncConverterConfigureAwaitSettings, IIndexedEntry<string, string>>>
            ConfigureAwaitIgnoreAttributeTypes =
                x => x.ConfigureAwaitIgnoreAttributeTypes;
    }
}