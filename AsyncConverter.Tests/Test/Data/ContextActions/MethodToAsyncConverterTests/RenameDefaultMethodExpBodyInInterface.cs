using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public interface IClass
    {
        void {caret}Test() => Run();

        void Run();
        Task RunAsync();
    }
}
