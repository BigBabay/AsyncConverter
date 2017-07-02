using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync(int input) => await Task.Delay(1000).ConfigureAwait(false);
    }
}
