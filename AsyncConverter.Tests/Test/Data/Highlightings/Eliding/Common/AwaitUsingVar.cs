using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public abstract class Class
    {
        public async Task TestAsync()
        {
            await using var usingVar = GetAsyncDisposable();
            await Task.Delay(0).ConfigureAwait(false);
        }

        public abstract IAsyncDisposable GetAsyncDisposable();
    }
}
