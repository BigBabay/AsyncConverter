using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Foo().ConfigureAwait(false);
            async Task<int> Foo() => {caret}await MethodAsync().ConfigureAwait(false);
        }

        public Task<int> MethodAsync()
        {
            return Task.FromResult(5);
        }
    }
}
