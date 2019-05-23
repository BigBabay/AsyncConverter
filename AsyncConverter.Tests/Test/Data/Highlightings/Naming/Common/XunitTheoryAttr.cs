using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    using Xunit;
    public class Class
    {
        [Theory]
        public async Task Test()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            await Task.Delay(1000).ConfigureAwait(false);
        }
    }
}

namespace Xunit
{
    public class TheoryAttribute : Attribute
    { }
}