using System.Collections.Generic;
using JetBrains.Annotations;

namespace AsyncConverter.Helpers
{
    public static class EnumerableExtension
    {
        [NotNull]
        public static HashSet<TItem> ToHashSet<TItem>([NotNull] this IEnumerable<TItem> items) => new HashSet<TItem>(items);
    }
}