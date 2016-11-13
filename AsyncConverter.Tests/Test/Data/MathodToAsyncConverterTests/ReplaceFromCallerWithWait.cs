using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public void {caret}Test()
        {
        }
        public void Caller()
        {
            Test();
        }
    }
}
