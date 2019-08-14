using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        private readonly object lockObject = new object();
        public async Task TestAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            lock (lockObject)
            {
                Method();                
            }
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
