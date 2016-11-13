using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        private IArg arg;

        public void {caret}Test()
        {
            var a = Method(x => x.InnerMethod());
        }

        public int Method(Func<IArg, int> func)
        {
            return func(arg);
        }

        public Task<int> MethodAsync(Func<IArg, Task<int>> func)
        {
            return func(arg);
        }
    }

    public interface IArg
    {
        int InnerMethod();
        Task<int> InnerMethodAsync();
    }
}
