using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.QuickFixes
{
    [TestFixture]
    public class FixReturnValueToTaskTests : CSharpQuickFixTestBase<ReturnValueToTask>
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
