using System.IO;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AsyncConverter.Tests.QuickFixes
{
    [TestNetFramework46]
    public abstract class QuickFixBaseTests<TQuickFix> : CSharpQuickFixTestBase<TQuickFix> where TQuickFix : IQuickFix
    {
        [TestCaseSource(nameof(FileNames))]
        public void Test(string fileName)
        {
            DoTestFiles(fileName);
        }

        protected abstract string Folder { get; }

        protected sealed override string RelativeTestDataPath => "QuickFixes\\" + Folder;

        // ReSharper disable once MemberCanBePrivate.Global
        protected TestCaseData[] FileNames()
        {
            return Directory
                .GetFiles(@"..\..\..\..\Test\Data\" + RelativeTestDataPath, "*.cs")
                .Select(x => new TestCaseData(Path.GetFileName(x)))
                .ToArray();
        }
    }
}