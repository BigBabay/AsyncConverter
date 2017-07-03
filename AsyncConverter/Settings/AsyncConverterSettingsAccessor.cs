using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace AsyncConverter.Settings
{
    internal static class AsyncConverterSettingsAccessor
    {
        [NotNull]
        public static readonly Expression<Func<AsyncConverterNamingSettings, bool>>
            ExcludeTestMethodsFromAnalysis = x => x.ExcludeTestMethodsFromAnalysis;
    }
}