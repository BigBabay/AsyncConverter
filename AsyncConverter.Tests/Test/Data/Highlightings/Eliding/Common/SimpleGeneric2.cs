﻿using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public static async Task<int> LalalaAsync()
        {
            var task = Task.FromResult(5);
            return await task;
        }
    }
}
