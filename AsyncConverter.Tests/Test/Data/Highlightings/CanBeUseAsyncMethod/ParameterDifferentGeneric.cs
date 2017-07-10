using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            Method(6);
        }

        public void Method<T>(T i)
        {
        }

        public Task MethodAsync(int i)
        {
            return Task.CompletedTask;
        }
    }
}
