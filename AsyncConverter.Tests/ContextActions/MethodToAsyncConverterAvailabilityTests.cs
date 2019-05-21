using AsyncConverter.ContextActions;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.ContextActions
{
    [TestNetFramework45]
    public class MethodToAsyncConverterAvailabilityTests : CSharpContextActionAvailabilityTestBase<MethodToAsyncConverter>
    {
        protected override string ExtraPath => "MethodToAsyncConverterAvailabilityTests";

        protected override string RelativeTestDataPath => "MethodToAsyncConverterAvailabilityTests";

        [Test]
        public void Test()
        {
            DoTestFiles("Test01.cs");
        }
    }
}