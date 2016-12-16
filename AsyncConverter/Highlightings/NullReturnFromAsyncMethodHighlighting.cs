using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("NullReturnFromAsyncMethod", null, "CodeInfo", "Some title", "Other title", Severity.WARNING, false)]
namespace AsyncConverter.Highlightings
{
    [ConfigurableSeverityHighlighting("NullReturnFromAsyncMethod", "CSHARP")]
    public class NullReturnAsTaskHighlighting : IHighlighting
    {
        private readonly ICSharpLiteralExpression cSharpLiteralExpression;
        private readonly IType returnType;

        public NullReturnAsTaskHighlighting(ICSharpLiteralExpression cSharpLiteralExpression, IType returnType)
        {
            this.cSharpLiteralExpression = cSharpLiteralExpression;
            this.returnType = returnType;
        }

        public string ToolTip => "Null return as Task";
        public string ErrorStripeToolTip => "May cause null reference if Task will be await.";

        public bool IsValid()
        {
            return true;
        }

        public DocumentRange CalculateRange()
        {
            return cSharpLiteralExpression.GetDocumentRange();
        }
    }
}
