using System.IO;
using System.Linq;
using AsyncConverter.QuickFixes;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;

namespace AsyncConverter.Tests.QuickFixes
{
    [TestFixture]
    public class ReturnNullToTaskTests : CSharpQuickFixTestBase<ReturnNullToTask>
    {
        protected override string RelativeTestDataPath => @"ReturnNullToTaskTests";

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

    [TestFixture]
    public class ReturnNullToTaskAvailabilityTests : QuickFixAvailabilityTestBase
    {
        protected override string RelativeTestDataPath => @"ReturnNullToTaskTests";

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