using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task<object> TestAsync()
        {
            await Test2Async();
            return null;
        }

        public Task Test2Async()
        {
            return Task.CompletedTask;
        }
    }
}
