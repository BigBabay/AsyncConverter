using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public static class Enumerable
    {
        public static T[] ToArray<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToArray();
        }
    }

    public static class QueryableExtensions
    {
        public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.ToArray());
        }
    }

    public class Class
    {
        public T[] {caret}Test(IQueryable<T> queryable)
        {
            return queryable.ToArray();
        }
    }
}

