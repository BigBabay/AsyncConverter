using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public Task<int> Main()
        {
            return Task.FromResult(0);
        }
    }
}
