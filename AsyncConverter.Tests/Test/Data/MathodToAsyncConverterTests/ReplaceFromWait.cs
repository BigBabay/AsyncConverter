using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public void {caret}Test()
        {
            MethodAsync().Wait();
        }

        public async Task MethodAsync()
        {
        }
    }
}
