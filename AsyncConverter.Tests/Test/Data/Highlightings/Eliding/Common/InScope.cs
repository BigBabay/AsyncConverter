using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync(int input)
        {
            if (input == 5)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }
            Console.WriteLine();
        }
    }
}
