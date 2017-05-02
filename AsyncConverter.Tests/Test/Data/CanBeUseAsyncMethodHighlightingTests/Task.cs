using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public Task TestAsync()
        {
            Method();
            return Task.CompletedTask;
        }

        public void Method()
        {
        }

        public Task MethodAsync()
        {
            return Task.CompletedTask;
        }
    }
}
