using System.IO;
using System.Linq;
using AsyncConverter.QuickFixes;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.QuickFixes
{
    [TestFixture]
    [TestNetFramework4]
    public class AsyncMethodNamingQuickFixTests : CSharpQuickFixTestBase<AsyncMethodNamingQuickFix>
    {
        protected override string RelativeTestDataPath => @"NamingQuickFix";

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