using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public interface IClass
    {
        public int Test();
    }

    public class Class1
    {
        public virtual int Test()
        {
            return 5;
        }
    }

    public class Class2 : Class1, IClass
    {
        public override int {caret}Test()
        {
            return 5;
        }
    }
}