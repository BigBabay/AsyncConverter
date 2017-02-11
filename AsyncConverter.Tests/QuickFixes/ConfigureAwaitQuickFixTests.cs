using AsyncConverter.QuickFixes;

namespace AsyncConverter.Tests.QuickFixes
{
    public class ConfigureAwaitQuickFixTests : QuickFixBaseTests<ConfigureAwaitQuickFix>
    {
        protected override string RelativeTestDataPath => @"ConfigureAwaitQuickFixTests";
    }
}