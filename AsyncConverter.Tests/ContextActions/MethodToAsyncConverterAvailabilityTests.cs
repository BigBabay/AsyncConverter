using AsyncConverter.ContextActions;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.ContextActions
{
    [TestNetFramework45]
    public class MethodToAsyncConverterAvailabilityTests : CSharpContextActionAvailabilityTestBase<MethodToAsyncConverter>
    {
        protected override string ExtraPath => "";

        protected override string RelativeTestDataPath => @"ContextActions\MethodToAsyncConverterAvailabilityTests";

        [Test]
        public void Test()
        {
            DoTestSolution("Test01.cs");
        }
    }
}
