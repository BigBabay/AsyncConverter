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

        public async Task<string> TestAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return service.Method("lalala").AnotherMethod("bububu");
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
