using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("NullReturnFromAsyncMethod", null, "CodeInfo", "Some title", "Other title", Severity.WARNING, false)]
namespace AsyncConverter.Highlightings
{
    [ConfigurableSeverityHighlighting("NullReturnFromAsyncMethod", "CSHARP")]
    public class NullReturnAsTaskHighlighting : IHighlighting
    {
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
            if (!CSharpLiteralExpression.IsValid() || !ReturnType.IsValid())
                return false;

            if (CSharpLiteralExpression.Literal.GetTokenType() != CSharpTokenType.NULL_KEYWORD)
                return false;

            if (!ReturnType.IsTask() && !ReturnType.IsGenericTask())
                return false;

            return true;
        }

        public DocumentRange CalculateRange()
        {
            return CSharpLiteralExpression.GetDocumentRange();
        }
    }
}
