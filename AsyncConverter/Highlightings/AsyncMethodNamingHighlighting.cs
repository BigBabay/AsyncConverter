using AsyncConverter.Highlightings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(AsyncMethodNamingHighlighting.SeverityId, null, AsyncConverterGroupSettings.Id, "Async method must ends on \"Async\"", "Async method must ends on \"Async\"", Severity.WARNING)]

namespace AsyncConverter.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name)]
    public class AsyncMethodNamingHighlighting : IHighlighting
    {
        public IMethodDeclaration MethodDeclaration { get; }
        public const string SeverityId = "AsyncConverter.AsyncMethodNamingHighlighting";

        public AsyncMethodNamingHighlighting(IMethodDeclaration methodDeclaration)
        {
            this.MethodDeclaration = methodDeclaration;
        }

        public bool IsValid()
        {
            return MethodDeclaration.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return MethodDeclaration.NameIdentifier.GetDocumentRange();
        }

        public string ToolTip { get; }
        public string ErrorStripeToolTip => "Async method must ends on \"Async\"";
    }
}