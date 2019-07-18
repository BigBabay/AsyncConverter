using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task<int> TestAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return Task.FromRe{caret}sult(1000).Result;
        }
    }
}
