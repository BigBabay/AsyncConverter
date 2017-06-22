using AsyncConverter.Highlightings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(AwaitForgottenInUsingHighlighting.SeverityId, null, AsyncConverterGroupSettings.Id, "Await Task in using", "Task<IDisposable> without awaiting in using may cause resource leak.", Severity.WARNING)]

namespace AsyncConverter.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name)]
    public class AwaitForgottenInUsingHighlighting : IHighlighting
    {
        public IAwaitExpression AwaitExpression { get; }
        public const string SeverityId = "AsyncConverter.AwaitForgottenInUsingHighlighting";

        public AwaitForgottenInUsingHighlighting(IAwaitExpression awaitExpression)
        {
            AwaitExpression = awaitExpression;
        }

        public bool IsValid() => AwaitExpression.IsValid();

        public DocumentRange CalculateRange() => AwaitExpression.GetDocumentRange();

        public string ToolTip => "Object will not be disposed because Task not awaiting";
        public string ErrorStripeToolTip => "Task with IDisposable not awaiting";
    }
}