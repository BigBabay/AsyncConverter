using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public interface IClass
    {
        void Test();
    }

    public class Class : IClass
    {
        public void {caret}Test()
        {
            return 5;
        }
    }
}
