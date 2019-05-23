using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            {caret}Method();
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
