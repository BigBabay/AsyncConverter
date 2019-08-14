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
            return service.Method("lalala");
        }
    }

    public interface IService
    {
        string Method(string s);
        Task<string> MethodAsync(string s);
    }
}
