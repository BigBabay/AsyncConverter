using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public Task<int> MethodAsync(Task<int> taskInt)
        {
            return {caret}5;
        }
    }
}
