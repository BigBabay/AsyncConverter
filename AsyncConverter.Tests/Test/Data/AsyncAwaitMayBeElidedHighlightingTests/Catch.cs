using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }
    }
}
