using AsyncConverter.ContextActions;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;

namespace AsyncConverter.Tests.ContextActions
{
    [TestFixture]
    public class MathodToAsyncConverterAvailabilityTests : CSharpContextActionAvailabilityTestBase<MathodToAsyncConverter>
    {
        protected override string ExtraPath => "MathodToAsyncConverterAvailabilityTests";

        protected override string RelativeTestDataPath => "MathodToAsyncConverterAvailabilityTests";

        [Test]
        public void Test()
        {
            DoTestFiles("Test01.cs");
        }
    }
}