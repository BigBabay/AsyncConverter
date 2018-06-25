using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.Highlightings.ConfigureAwaitWithAttribute
{
    public class Class
    {
        [MyCustom]
        public async Task TestAsync()
        {
            await Task.Delay(1000);
        }
    }

    public class MyCustomAttribute : Attribute
    {

    }
}
