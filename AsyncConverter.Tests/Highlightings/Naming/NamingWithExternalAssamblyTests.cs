using JetBrains.ReSharper.TestFramework;

namespace AsyncConverter.Tests.Highlightings.Naming
{
    [TestReferences("LibraryToOverride.dll")]
    public class NamingWithExternalAssamblyTests : HighlightingsTestsBase
    {
        protected override string Folder => "Naming/WithOverride";
    }
}