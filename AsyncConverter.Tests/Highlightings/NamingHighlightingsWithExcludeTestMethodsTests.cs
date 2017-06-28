using AsyncConverter.Settings;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings
{
    [TestSetting(typeof(AsyncConverterSettings), "ExcludeTestMethodsFromAnalysis", true)]
    public class NamingHighlightingsWithExcludeTestMethodsTests : HighlightingsTestsBase
    {
        protected override string RelativeTestDataPath => "NamingWithExcludeTestMethods";

    }
}