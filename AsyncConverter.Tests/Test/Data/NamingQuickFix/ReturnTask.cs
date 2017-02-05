using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public Task {caret}Test()
        {
            return Task.CompletedTask;
        }
    }
}
