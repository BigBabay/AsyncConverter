using AsyncConverter.Settings.General;
using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings.Naming
{
    [TestSetting(typeof(GeneralSettings), nameof(GeneralSettings.ExcludeTestMethodsFromRanaming), false)]
    public class NamingWithExcludeTestMethodsTests : HighlightingsTestsBase
    {
        protected override string Folder => "Naming/WithExcludeTest";
    }
}