using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public Task Main(int[] args)
        {
            return Task.CompletedTask;
        }
    }
}
