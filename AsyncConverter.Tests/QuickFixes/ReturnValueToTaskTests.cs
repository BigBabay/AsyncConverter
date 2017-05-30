using AsyncConverter.QuickFixes;

namespace AsyncConverter.Tests.QuickFixes
{
    public class ReturnValueToTaskTests : QuickFixBaseTests<ReturnValueAsTask>
    {
        protected override string RelativeTestDataPath => @"ReturnValueAsTaskTests";
    }
}
