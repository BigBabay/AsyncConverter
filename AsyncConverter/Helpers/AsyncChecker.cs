using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class AsyncChecker : IAsyncChecker
    {
        public bool CanUseAwait(IInvocationExpression invocation)
        {
            var containingFunctionDeclarationIgnoringClosures = invocation?.InvokedExpression.GetContainingFunctionDeclarationIgnoringClosures();
            if (containingFunctionDeclarationIgnoringClosures == null)
                return false;

            return containingFunctionDeclarationIgnoringClosures.IsAsync;
        }
    }
}