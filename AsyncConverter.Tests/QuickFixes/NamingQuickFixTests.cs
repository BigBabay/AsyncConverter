using AsyncConverter.QuickFixes;

namespace AsyncConverter.Tests.QuickFixes
{
    public class NamingQuickFixTests : QuickFixBaseTests<AsyncMethodNamingQuickFix>
    {
        protected override string RelativeTestDataPath => @"NamingQuickFix";
    }
}