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

        public Task<string> MethodAsync(int s)
        {
            return Task.FromResult("aaa");
        }
    }
}
