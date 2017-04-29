using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public interface IClass
    {
        public int Test();
    }

    public class Class1 : IClass
    {
        public int {caret}Test()
        {
            return 5;
        }
    }

    public class Class2 : IClass
    {
        public int Test()
        {
            return 5;
        }
    }
}