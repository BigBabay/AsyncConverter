using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Task.Factory.StartNew(async () =>
                                        {
                                            Console.WriteLine("foo");
                                            aw{caret}ait Task.Delay(1000);
                                        }
        }
    }
}
