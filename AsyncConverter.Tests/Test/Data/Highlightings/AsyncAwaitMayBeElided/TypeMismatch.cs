using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task<IList<int>> TestAsync()
        {
            var listTask = Task.FromResult(new List<int>());
            return await listTask.ConfigureAwait(false);
        }
    }
}
