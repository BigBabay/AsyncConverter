using AsyncConverter.Highlightings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(CanBeUseAsyncMethodHighlighting.SeverityId, null, AsyncConverterGroupSettings.Id, "May use non blocking async method instead sync method.", "May use non blocking async method instead sync method.", Severity.WARNING)]

namespace AsyncConverter.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name)]
    public class CanBeUseAsyncMethodHighlighting : IHighlighting
    {
        public IInvocationExpression InvocationExpression { get; }
        public const string SeverityId = "AsyncConverter.CanBeUseAsyncMethodHighlighting";

        public CanBeUseAsyncMethodHighlighting(IInvocationExpression invocationExpression)
        {
            InvocationExpression = invocationExpression;
        }

        public bool IsValid()
        {
            return InvocationExpression.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return InvocationExpression.GetDocumentRange();
        }

        public string ToolTip { get; }
        public string ErrorStripeToolTip => "May be converted to nonblocking await call.";
    }
}