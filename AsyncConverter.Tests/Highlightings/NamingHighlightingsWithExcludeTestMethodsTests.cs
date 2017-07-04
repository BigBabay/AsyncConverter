using AsyncConverter.Settings.AsyncSuffixOptions;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings
{
    [TestSetting(typeof(AsyncConverterAsyncSuffixSettings), "ExcludeTestMethodsFromAnalysis", false)]
    public class NamingWithExcludeTestMethodsTests : HighlightingsTestsBase
    {
        protected override string Folder => "NamingWithExcludeTestMethods";

    }
}