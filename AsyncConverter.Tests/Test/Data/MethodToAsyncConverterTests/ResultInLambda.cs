using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public void {caret}Test()
        {
            var a = Execute(() => MethodAsync().Result);
        }

        private int Execute(Func<int> func)
        {
            return func();
        }

        private Task<int> ExecuteAsync(Func<Task<int>> func)
        {
            return func();
        }

        public async Task<int> MethodAsync()
        {
            return 5;
        }
    }
}
