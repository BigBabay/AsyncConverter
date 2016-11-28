using System.IO;
using System.Linq;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;

namespace AsyncConverter.Tests
{
    [TestFixture]
    public class MathodToAsyncConverterTests : CSharpContextActionExecuteTestBase<MathodToAsyncConverter>
    {
        protected override string ExtraPath => "MathodToAsyncConverterTests";

        protected override string RelativeTestDataPath => "MathodToAsyncConverterTests";

        [TestCaseSource(nameof(FileNames))]
        public void Test(string fileName)
        {
            DoTestFiles(fileName);
        }

        private TestCaseData[] FileNames()
        {
            return Directory
                .GetFiles(@"..\..\Test\Data\" + RelativeTestDataPath, "*.cs")
                .Select(x => new TestCaseData(Path.GetFileName(x)))
                .ToArray();
        }
    }
}