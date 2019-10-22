using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public int TestAsync()
        {
            var a = Task.FromResult(1000).Result;
            return a;
        }
    }
}
