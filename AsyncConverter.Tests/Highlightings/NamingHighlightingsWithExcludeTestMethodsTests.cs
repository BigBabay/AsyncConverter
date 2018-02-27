using AsyncConverter.Settings.General;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings
{
    [TestSetting(typeof(GeneralSettings), nameof(GeneralSettings.ExcludeTestMethodsFromRanaming), false)]
    public class NamingWithExcludeTestMethodsTests : HighlightingsTestsBase
    {
        protected override string Folder => "NamingWithExcludeTestMethods";
    }
}