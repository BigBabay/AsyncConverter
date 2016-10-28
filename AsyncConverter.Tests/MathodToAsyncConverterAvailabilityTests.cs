using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;

namespace AsyncConverter.Tests
{
    [TestFixture]
    public class MathodToAsyncConverterAvailabilityTests : CSharpContextActionAvailabilityTestBase<MathodToAsyncConverter>
    {
        protected override string ExtraPath => "MathodToAsyncConverterTests";

        protected override string RelativeTestDataPath => "MathodToAsyncConverterTests";

        [Test]
        public void Test02()
        {
            DoTestFiles("Test01.cs");
        }
    }
}