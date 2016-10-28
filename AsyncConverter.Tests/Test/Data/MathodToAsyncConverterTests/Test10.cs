using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        private readonly IService service;

        public Class(IService service)
        {
            this.service = service;
        }

        public int {caret}Test()
        {
            var a = service.Method("lalala");
            return 4;
        }
    }

    public interface IService
    {
        string Method(string s);
        Task<string> MethodAsync(string s);
    }
}
