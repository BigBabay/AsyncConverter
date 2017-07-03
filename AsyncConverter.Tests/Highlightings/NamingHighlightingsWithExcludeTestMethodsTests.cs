using AsyncConverter.Settings;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings
{
    [TestSetting(typeof(AsyncConverterAsyncSuffixOptionsPage), "ExcludeTestMethodsFromAnalysis", false)]
    public class NamingWithExcludeTestMethodsTests : HighlightingsTestsBase
    {
        protected override string Folder => "NamingWithExcludeTestMethods";

    }
}