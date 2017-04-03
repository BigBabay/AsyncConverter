using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.ParameterComparers
{
    public static class AsyncExtensions
    {
        [Pure]
        public static bool IsUnderAsync([NotNull] this ITreeNode node)
        {
            foreach (var containingNode in node.ContainingNodes())
            {
                var methodDeclaration = containingNode as IMethodDeclaration;
                if (methodDeclaration != null)
                    return methodDeclaration.Type.IsTask() || methodDeclaration.Type.IsGenericTask();
                var functionExpression = containingNode as IAnonymousFunctionExpression;
                if (functionExpression != null)
                    return functionExpression.ReturnType.IsTask() || functionExpression.ReturnType.IsGenericTask();
                var functionDeclaration = containingNode as ILocalFunctionDeclaration;
                if (functionDeclaration != null)
                    return functionDeclaration.Type.IsTask() || functionDeclaration.Type.IsGenericTask();
                if (containingNode is IQueryParameterPlatform || containingNode is ICSharpTypeMemberDeclaration)
                    return false;
            }
            return false;
        }
    }
}