using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task<object> Test()
        {
            await Test2();
            return null;
        }

        public Task Test2()
        {
            return Task.Delay(0);
        }
    }
}
