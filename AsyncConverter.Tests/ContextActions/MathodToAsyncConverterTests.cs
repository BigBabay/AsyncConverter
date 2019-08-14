using AsyncConverter.ContextActions;
using AsyncConverter.Tests.Helpers;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.ContextActions
{
    [TestNetFramework46]
    public class MethodToAsyncConverterTests : CSharpContextActionExecuteTestBase<MethodToAsyncConverter>
    {
        protected override string ExtraPath => "";

        protected override string RelativeTestDataPath => "ContextActions\\" + nameof(MethodToAsyncConverterTests);

        [TestCaseSource(typeof(TestHelper), nameof(TestHelper.FileNames), new object[]{@"ContextActions\" + nameof(MethodToAsyncConverterTests)})]
        public void Test(string fileName)
        {
            DoTestFiles(fileName);
        }
    }
}