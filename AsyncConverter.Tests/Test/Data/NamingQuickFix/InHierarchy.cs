using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public interface IInterface
    {
        public Task Test();
    }

    public class Class : IInterface
    {
        public virtual Task {caret}Test()
        {
            return Task.CompletedTask;
        }
    }

    public class Class2 : Class
    {
        public override Task Test()
        {
            return Task.CompletedTask;
        }
    }
}
