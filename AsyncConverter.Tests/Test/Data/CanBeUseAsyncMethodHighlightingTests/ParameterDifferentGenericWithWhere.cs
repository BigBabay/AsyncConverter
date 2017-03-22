using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            Method();
        }

        public void Method<T>(T i)
        {
        }

        public Task MethodAsync<T>(T i) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
