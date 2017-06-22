using System.IO;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task TestAsync()
        {
            using (MethodAsync())
            {

            }
        }

        private Task<IDisposable> MethodAsync()
        {
            throw new NotImplementedException();
        }
    }
}
