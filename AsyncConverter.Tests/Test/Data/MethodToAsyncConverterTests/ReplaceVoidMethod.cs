using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public void {caret}Test()
        {
            Method();
            var a = 6;
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
