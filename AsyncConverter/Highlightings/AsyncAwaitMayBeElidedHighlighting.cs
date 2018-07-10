using AsyncConverter.Highlightings;
using AsyncConverter.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(AsyncAwaitMayBeElidedHighlighting.SeverityId, null, AsyncConverterGroupSettings.Id, "Elide async/await", "Elide async/await if task may be returned", Severity.WARNING)]

namespace AsyncConverter.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name)]
    public class AsyncAwaitMayBeElidedHighlighting : IHighlighting
    {
        public IAwaitExpression AwaitExpression { get; }
        public const string SeverityId = "AsyncConverter.AsyncAwaitMayBeElidedHighlighting";

        public AsyncAwaitMayBeElidedHighlighting(IAwaitExpression awaitExpression)
        {
            AwaitExpression = awaitExpression;
        }

        public bool IsValid() => AwaitExpression.IsValid();

        public DocumentRange CalculateRange() => AwaitExpression.GetDocumentRange();

        public string ToolTip => "Async in method declaration and await may be elided.";
        public string ErrorStripeToolTip => "Await may be elided.";
    }
}