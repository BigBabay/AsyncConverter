using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Task.Factory.StartNew(async () => await Task.Delay(1000).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
