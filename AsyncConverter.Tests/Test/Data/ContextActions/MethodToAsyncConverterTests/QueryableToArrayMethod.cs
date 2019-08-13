using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Data.Entity
{
    public static class QueryableExtensions
    {
        public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.ToArray());
        }
    }
}

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public T[] {caret}Test(IQueryable<T> queryable)
        {
            var array = queryable.ToArray();
            return array;
        }
    }
}

