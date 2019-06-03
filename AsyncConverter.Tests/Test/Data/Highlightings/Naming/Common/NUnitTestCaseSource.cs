using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    using NUnit.Framework;
    public class Class
    {
        [TestCaseSource(nameof(FileNames))]
        public async Task Test()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            await Task.Delay(1000).ConfigureAwait(false);
        }

        protected TestCaseData[] FileNames()
        {
            return new TestCaseData[0];
        }
    }
}

namespace NUnit.Framework
{
    public class TestCaseSourceAttribute : Attribute
    {
        public TestCaseSourceAttribute(string sourceName)
        {

        }
    }

    public class TestCaseData
    {
    }
}