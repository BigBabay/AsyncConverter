﻿using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Task.Factory.StartNew(async () => aw{caret}ait MethodAsync());
        }

        public Task<int> MethodAsync()
        {
            return Task.FromResult(5);
        }
    }
}
