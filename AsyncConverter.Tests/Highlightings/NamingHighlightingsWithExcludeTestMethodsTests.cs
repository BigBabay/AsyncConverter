using AsyncConverter.Settings;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings
{
    [TestSetting(typeof(AsyncConverterSettings), "ExcludeTestMethodsFromAnalysis", false)]
    public class NamingWithExcludeTestMethodsTests : HighlightingsTestsBase
    {
        protected override string Folder => "NamingWithExcludeTestMethods";

    }
}