using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace AsyncConverter.Settings
{
    internal static class AsyncSuffixSettingsAccessor
    {
        [NotNull]
        public static readonly Expression<Func<AsyncConverterSettings, bool>>
            ExcludeTestMethodsFromAnalysis = x => x.ExcludeTestMethodsFromAnalysis;
    }
}