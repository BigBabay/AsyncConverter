using AsyncConverter.Settings.General;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings.Eliding
{
    [TestSetting(typeof(GeneralSettings), nameof(GeneralSettings.ExcludeTestMethodsFromEliding), false)]
    public class ElidingWithExcludeTestMethodsTests : HighlightingsTestsBase
    {
        protected override string Folder => "ElidingWithWithExcludeTestMethods";
    }
}