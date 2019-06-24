using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public Task<long> Main()
        {
            return Task.FromResult(0L);
        }
    }
}
