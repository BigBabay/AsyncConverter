using System.Threading.Tasks;
using LibraryToOverride;

namespace AsyncConverter.Tests
{
    public class Class : IInterfaceToOverride
    {
        public Task Method()
        {
            return Task.CompletedTask;
        }
    }
}