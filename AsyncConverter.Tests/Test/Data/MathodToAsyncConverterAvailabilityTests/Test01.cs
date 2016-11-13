using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public void {on}T{on}est{on}()
        {
            var a{off} = Method();
        }

        public i{off}nt Met{on}hod()
        {
            return 5;
        }

        public Task<int> Met{off}hodAsync()
        {
            return Task.FromRe{off}sult(5);
        }
    }
}
