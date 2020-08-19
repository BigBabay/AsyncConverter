using AsyncConverter.QuickFixes;
using AsyncConverter.Tests.Helpers;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.QuickFixes
{
    [TestNetFramework46]
    public class ElidingTests : CSharpQuickFixTestBase<AsyncAwaitMayBeElidedQuickFix>
    {
        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[]{@"QuickFixes\" + nameof(ElidingTests)})]
        public void Test(string fileName)
        {
            DoTestSolution(fileName);
        }
    }
}
