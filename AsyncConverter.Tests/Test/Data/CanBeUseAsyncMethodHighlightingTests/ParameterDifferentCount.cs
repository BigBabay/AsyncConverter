using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            Method(5, "lalala");
        }

        public void Method(int i, string a)
        {
        }

        public Task MethodAsync(int i)
        {
            return Task.CompletedTask;
        }
    }
}
