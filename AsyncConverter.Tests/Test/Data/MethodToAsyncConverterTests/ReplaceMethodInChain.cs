using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        private readonly IService service;
        private readonly IAnotherService anotherService;

        public Class(IService service, IAnotherService anotherService)
        {
            this.service = service;
            this.anotherService = anotherService;
        }

        public int {caret}Test()
        {
            var a = service.Method("lalala").AnotherMethod("bububu");
            return 4;
        }
    }

    public interface IService
    {
        IAnotherService Method(string s);
        Task<IAnotherService> MethodAsync(string s);
    }

    public interface IAnotherService
    {
        string AnotherMethod(string s);
        Task<string> AnotherMethodAsync(string s);
    }
}
