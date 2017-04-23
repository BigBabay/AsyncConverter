using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public int {caret}Test()
        {
            var a = Method();
            var b = Method2();
            return 2;
        }

        public int Method()
        {
            return 5;
        }

        public Task<int> MethodAsync()
        {
            return Task.FromResult(5);
        }

        public int Method2()
        {
            return 5;
        }

        public Task<int> Method2Async()
        {
            return Task.FromResult(5);
        }
    }
}
