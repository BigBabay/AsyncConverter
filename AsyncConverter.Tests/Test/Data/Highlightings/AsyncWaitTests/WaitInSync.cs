﻿using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public void TestAsync()
        {
            Task.Delay(1000).Wait();
        }
    }
}
