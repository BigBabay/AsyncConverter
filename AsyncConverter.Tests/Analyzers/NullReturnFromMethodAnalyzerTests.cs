using System.IO;
using System.Linq;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using NUnit.Framework;

namespace AsyncConverter.Tests.Analyzers
{
    [TestFixture]
    public class NullReturnAnalyzerTests : CSharpHighlightingTestBase
    {
        protected override string RelativeTestDataPath => "NullReturnAnalyzerTests";

        [TestCaseSource(nameof(FileNames))]
        public void Test(string fileName)
        {
            DoTestSolution(fileName);
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