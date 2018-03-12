using AsyncConverter.QuickFixes;

namespace AsyncConverter.Tests.QuickFixes
{
    public class AsyncMethodNamingQuickFixTests : QuickFixBaseTests<AsyncMethodNamingQuickFix>
    {
        protected override string RelativeTestDataPath => @"NamingQuickFix";
    }
}