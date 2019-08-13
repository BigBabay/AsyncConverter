using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public int {caret}Test()
        {
            var a = Method();
        }

        public int Method()
        {
            return 5;
        }

        public Task<int> MethodAsync()
        {
            return Task.FromResult(5);
        }
    }
}
