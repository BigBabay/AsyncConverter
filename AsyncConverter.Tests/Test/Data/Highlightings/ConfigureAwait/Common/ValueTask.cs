using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Aaa().ConfigureAwait(false);
        }
        
        public static ValueTask<int> Aaa()
        {
            return new ValueTask<int>(5);
        }
    }
}
