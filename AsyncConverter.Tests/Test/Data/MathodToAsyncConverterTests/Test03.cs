namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    using System;

    public class Class
    {
        public int {caret}Test()
        {
            var a = Method();
        }
    }
}
