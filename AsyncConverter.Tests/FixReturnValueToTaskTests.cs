using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;

namespace AsyncConverter.Tests
{
    [TestFixture]
    public class FixReturnValueToTaskTests : CSharpQuickFixTestBase<FixReturnValueToTask>
    {
        protected override string RelativeTestDataPath => @"FixReturnValueToTaskTests";

        [Test]
        public void Test01()
        {
            DoTestFiles("Test01.cs");
        }
    }
}
