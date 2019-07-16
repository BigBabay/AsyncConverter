using AsyncConverter.Highlightings;
using AsyncConverter.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(AsyncWaitHighlighting .SeverityId, null, AsyncConverterGroupSettings.Id, "Use async wait instead sync wait.", "Use async wait instead sync wait.", Severity.ERROR)]

namespace AsyncConverter.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name)]
    public class AsyncWaitHighlighting : IHighlighting
    {
        public IReferenceExpression InvocationExpression { get; }
        public const string SeverityId = "AsyncConverter.AsyncWait";

        public AsyncWaitHighlighting(IReferenceExpression invocationExpression)
        {
            InvocationExpression = invocationExpression;
        }

        public bool IsValid() => InvocationExpression.IsValid();

        public DocumentRange CalculateRange() => InvocationExpression.GetDocumentRange();

        public string ToolTip => "Use async wait instead sync wait.";
        public string ErrorStripeToolTip => "Use async wait.";
    }
}