using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task<int> TestAsync(bool a)
        {
            if(a)
                return 5;

            return await FooAsync();
        }

        public Task<int> FooAsync()
        {
            return Task.FromResult(5);
        }
    }
}
