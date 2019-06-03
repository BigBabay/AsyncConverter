using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    using Microsoft.AspNetCore.Mvc;
    public class Class : ControllerBase
    {
        public async Task Test()
        {
            await Task.Delay(1000).ConfigureAwait(false);
        }
    }
}

namespace Microsoft.AspNetCore.Mvc
{
    public class ControllerBase
    { }
}