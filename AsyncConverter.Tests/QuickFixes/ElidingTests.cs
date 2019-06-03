using AsyncConverter.QuickFixes;

namespace AsyncConverter.Tests.QuickFixes
{
    public class AsyncAwaitMayBeElidedQuickFixTests : QuickFixBaseTests<AsyncAwaitMayBeElidedQuickFix>
    {
        protected override string Folder => @"Eliding";
    }
}