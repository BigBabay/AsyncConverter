using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task<int> TestAsync()
        {
            var withResult = new WithResult();
            await Task.Delay(1000).ConfigureAwait(false);
            return withResult.Result;
        }
    }

    public class WithResult
    {
        public int Result { get; set; }
    }
}