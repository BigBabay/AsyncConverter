using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            var i = 5;
            do
            {
                await Task.Delay(1000).ConfigureAwait(false);
                i--;
            } while (i > 0);
        }
    }
}
