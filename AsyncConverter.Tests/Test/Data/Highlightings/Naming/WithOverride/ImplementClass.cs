using System.Threading.Tasks;
using LibraryToOverride;

namespace AsyncConverter.Tests
{
    public class Class : ClassToOverride
    {
        protected override Task Method()
        {
            return Task.CompletedTask;
        }
    }
}