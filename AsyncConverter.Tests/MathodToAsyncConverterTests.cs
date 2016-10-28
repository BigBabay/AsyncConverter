using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;

namespace AsyncConverter.Tests
{
    [TestFixture]
    public class MathodToAsyncConverterTests : CSharpContextActionExecuteTestBase<MathodToAsyncConverter>
    {
        protected override string ExtraPath => "MathodToAsyncConverterTests";

        protected override string RelativeTestDataPath => "MathodToAsyncConverterTests";

        [Test]
        public void ReplaceToGenericTask()
        {
            DoTestFiles("Test01.cs");
        }

        [Test]
        public void AddingUsing()
        {
            DoTestFiles("Test02.cs");
        }

        [Test]
        public void AddingUsingInNamespace()
        {
            DoTestFiles("Test03.cs");
        }

        [Test]
        public void ReplaceToTask()
        {
            DoTestFiles("Test04.cs");
        }

        [Test]
        public void ReplaceMethod()
        {
            DoTestFiles("Test05.cs");
        }

        [Test]
        public void ReplaceMethodWithParameter()
        {
            DoTestFiles("Test06.cs");
        }

        [Test]
        public void ReplaceMethodWithCorrectChosenName()
        {
            DoTestFiles("Test07.cs");
        }

        [Test]
        public void CorrectCompareParam()
        {
            DoTestFiles("Test08.cs");
        }

        [Test]
        public void CorrectCompareReturnType()
        {
            DoTestFiles("Test09.cs");
        }

        [Test]
        public void ReplaceMethodFromAnotherClass()
        {
            DoTestFiles("Test10.cs");
        }
    }
}