using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public interface ICalculator
    {
        Task<int> Sum(int first, int second);
        Task<int> Sum(int[] values);
    }

    public class Calculator
    {
        public async Task<int> Sum(int first, int second)
        {
            await Task.Delay(10);
            return first + second;
        }

        public async Task<int> Su{caret}m(int[] values)
        {
            await Task.Delay(10);
            return values.Sum();
        }
    }
}
