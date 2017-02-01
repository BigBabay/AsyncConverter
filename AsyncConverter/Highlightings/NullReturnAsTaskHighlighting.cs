using AsyncConverter.Highlightings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(NullReturnAsTaskHighlighting.SeverityId, null, AsyncConverterGroupSettings.Id, "Null return from async method", "May cause null reference exception if return of method will be awaiting.", Severity.WARNING)]

namespace AsyncConverter.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name)]
    public class NullReturnAsTaskHighlighting : IHighlighting
    {
        public const string SeverityId = "AsyncConverter.NullReturnAsTask";

        public ICSharpLiteralExpression CSharpLiteralExpression { get; }
        public IType ReturnType { get; }

        public NullReturnAsTaskHighlighting(ICSharpLiteralExpression cSharpLiteralExpression, IType returnType)
        {
            CSharpLiteralExpression = cSharpLiteralExpression;
            ReturnType = returnType;
        }

        public string ToolTip => "Null return as Task";
        public string ErrorStripeToolTip => "May cause null reference if Task will be await.";

        public bool IsValid()
        {
            return CSharpLiteralExpression.IsValid() && ReturnType.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return CSharpLiteralExpression.GetDocumentRange();
        }
    }
}
