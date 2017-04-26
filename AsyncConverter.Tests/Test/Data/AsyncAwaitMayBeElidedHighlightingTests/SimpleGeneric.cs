using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task<int> TestAsync()
        {
            return await MethodAsync().ConfigureAwait(false);
        }

        public Task<int> MethodAsync()
        {
            return Task.FromResult(5);
        }
    }
}
