using System;
using System.Threading.Tasks;

[assembly: AsyncConverter.Tests.Test.Data.Highlightings.ConfigureAwaitWithAttribute.MyCustom]

namespace AsyncConverter.Tests.Test.Data.Highlightings.ConfigureAwaitWithAttribute
{
    public class Class
    {
        public async Task TestAsync()
        {
            await Task.Delay(1000);
        }
    }

    public class MyCustomAttribute : Attribute
    {

    }
}
