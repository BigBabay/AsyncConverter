using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public void {caret}Test()
        {
            Method();
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
