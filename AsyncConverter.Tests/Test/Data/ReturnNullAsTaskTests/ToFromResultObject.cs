using System.Collections;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public Task<object> Test()
        {
            return {caret}null;
        }
    }
}