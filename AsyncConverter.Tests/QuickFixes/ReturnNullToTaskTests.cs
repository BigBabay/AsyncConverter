using AsyncConverter.QuickFixes;
using AsyncConverter.Tests.Helpers;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.QuickFixes
{
    [TestNetFramework46]
    public class ReturnNullAsTaskTests : CSharpQuickFixTestBase<ReturnNullAsTask>
    {
        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[]{@"QuickFixes\" + nameof(ReturnNullAsTaskTests)})]
        public void Test(string fileName)
        {
            DoTestFiles(fileName);
        }
    }
}