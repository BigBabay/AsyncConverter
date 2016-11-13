using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public virtual void Test()
        {
            var a = Method();
        }

        public int Method()
        {
            return 5;
        }

        public async Task<int> MethodAsync()
        {
            return 5;
        }
    }

    public class Class2 : Class
    {
        public override void {caret}Test()
        {
            var a = Method();
        }
    }
}
