using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Helpers
{
    public interface IAsyncInvocationReplacer
    {
        void ReplaceInvocation([CanBeNull] IInvocationExpression invocation, [NotNull] string newMethodName, bool useAwait);
    }
}