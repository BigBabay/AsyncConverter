using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            Method(() => Task.Delay(1000));
        }

        public void Method(Action action)
        {
            action();
        }

        public Task MethodAsync(Func<Task> func)
        {
            return func();
        }
    }
}
