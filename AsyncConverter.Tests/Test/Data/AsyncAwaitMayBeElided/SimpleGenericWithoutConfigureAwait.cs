using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task<int> TestAsync()
        {
            return {caret}await MethodAsync();
        }

        public Task<int> MethodAsync()
        {
            return Task.FromResult(5);
        }
    }
}
