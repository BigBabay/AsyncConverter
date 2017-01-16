using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests
{
    [TestFixture]
    public class FixReturnValueToTaskTests : CSharpQuickFixTestBase<FixReturnValueToTask>
    {
        protected override string RelativeTestDataPath => @"FixReturnValueToTaskTests";

        [Test]
        [TestNetFramework46]
        public void Test01()
        {
            DoTestFiles("Test01.cs");
        }
    }
}
