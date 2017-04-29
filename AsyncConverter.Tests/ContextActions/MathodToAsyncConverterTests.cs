using System.IO;
using System.Linq;
using AsyncConverter.ContextActions;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;

namespace AsyncConverter.Tests.ContextActions
{
    [TestFixture]
    public class MethodToAsyncConverterTests : CSharpContextActionExecuteTestBase<MethodToAsyncConverter>
    {
        protected override string ExtraPath => "MethodToAsyncConverterTests";

        protected override string RelativeTestDataPath => "MethodToAsyncConverterTests";

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