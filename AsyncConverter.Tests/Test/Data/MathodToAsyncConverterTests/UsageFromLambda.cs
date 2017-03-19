using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public int {caret}Test()
        {
            return 5;
        }

        public async Task Caller()
        {
            var a = CallerWihtLambda(() => Test());
        }

        public int CallerWihtLambda(Func<int> func)
        {
            return func();
        }

        public Task CallerWihtLambdaAsync(Func<Task<int>> func)
        {
            return func();
        }
    }
}
