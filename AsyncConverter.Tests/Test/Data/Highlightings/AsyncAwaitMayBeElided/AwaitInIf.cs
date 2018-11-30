using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        private async Task MethodAsync()
        {
            if (await IsAsync().ConfigureAwait(false))
            {
            }
        }

        private static async Task<bool> IsAsync()
        {
            await Task.Delay(100).ConfigureAwait(false);
            return true;
        }
    }
}
