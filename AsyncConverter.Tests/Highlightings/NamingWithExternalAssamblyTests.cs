using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings
{
    [TestReferences("LibraryToOverride.dll")]
    public class NamingWithExternalAssamblyTests : HighlightingsTestsBase
    {
        protected override string Folder => "NamingWithOverride";
    }
}