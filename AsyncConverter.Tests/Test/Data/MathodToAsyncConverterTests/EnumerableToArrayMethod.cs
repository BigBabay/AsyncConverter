using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public static class EnumerableExtension
    {
        public static T[] ToArray<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToArray();
        }
    }

    public static class QueryableExtension
    {
        public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> enumerable)
        {
            return Task.FromResult(enumerable.ToArray());
        }
    }

    public class Class
    {
        public T[] {caret}Test(IEnumerable<T> enumerable)
        {
            return enumerable.ToArray();
        }
    }
}

