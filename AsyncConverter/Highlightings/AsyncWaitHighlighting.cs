using AsyncConverter.Settings;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;


namespace AsyncConverter.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name)]
    [ RegisterConfigurableSeverity(AsyncWaitHighlighting.SeverityId, null, AsyncConverterGroupSettings.Id, "Use async wait instead sync wait.", "Use async wait instead sync wait.", Severity.ERROR)]
    public class AsyncWaitHighlighting : IHighlighting
    {
        public IInvocationExpression InvocationExpression { get; }
        public IReferenceExpression ReferenceExpression { get; }

        public const string SeverityId = "AsyncConverter.AsyncWait";

        public AsyncWaitHighlighting([NotNull] IReferenceExpression referenceExpression)
        {
            ReferenceExpression = referenceExpression;
        }

        public AsyncWaitHighlighting([NotNull] IInvocationExpression invocationExpression)
        {
            InvocationExpression = invocationExpression;
        }

        public bool IsValid() => ReferenceExpression?.IsValid() ?? InvocationExpression.IsValid();

        public DocumentRange CalculateRange() => ReferenceExpression?.GetDocumentRange() ?? InvocationExpression.GetDocumentRange();

        public string ToolTip => "Use async wait instead sync wait.";
        public string ErrorStripeToolTip => "Use async wait.";
    }
}