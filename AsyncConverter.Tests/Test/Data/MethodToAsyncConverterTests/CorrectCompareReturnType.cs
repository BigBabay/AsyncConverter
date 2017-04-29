using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public int {caret}Test()
        {
            var a = Method("lalala");
            return 4;
        }

        public string Method(string s)
        {
            return s;
        }

        public Task<int> MethodAsync(string s)
        {
            return Task.FromResult(5);
        }
    }
}
