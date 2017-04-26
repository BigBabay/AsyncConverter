using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers
{
    public static class InvocationExpressionExtensions
    {
        [Pure]
        [ContractAnnotation("null => null")]
        public static ICSharpExpression RemoveConfigureAwait(this IInvocationExpression expression)
        {
            ICSharpExpression expressionWithoutConfigureAwait;
            var xmlDocId = (expression?.Reference?.Resolve().Result.DeclaredElement as IMethod)?.XMLDocId;
            if (xmlDocId == "M:System.Threading.Tasks.Task.ConfigureAwait(System.Boolean)"
                || xmlDocId == "M:System.Threading.Tasks.Task`1.ConfigureAwait(System.Boolean)")
            {
                expressionWithoutConfigureAwait = (expression.FirstChild as IReferenceExpression)?.QualifierExpression;
                if (expressionWithoutConfigureAwait == null)
                    return null;
            }
            else
            {
                expressionWithoutConfigureAwait = expression;
            }
            return expressionWithoutConfigureAwait;
        }
    }
}