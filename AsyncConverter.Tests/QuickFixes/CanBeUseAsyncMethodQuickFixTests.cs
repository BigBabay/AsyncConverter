using AsyncConverter.QuickFixes;

namespace AsyncConverter.Tests.QuickFixes
{
    public class CanBeUseAsyncMethodQuickFixTests : QuickFixBaseTests<CanBeUseAsyncMethodQuickFix>
    {
        protected override string RelativeTestDataPath => @"CanBeUseAsyncMethodTests";
    }
}