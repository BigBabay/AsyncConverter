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
            if ((expression?.Reference?.Resolve().Result.DeclaredElement as IMethod)?.XMLDocId == "M:System.Threading.Tasks.Task.ConfigureAwait(System.Boolean)")
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