using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Helpers
{
    public interface IAsyncChecker
    {
        bool CanUseAwait([CanBeNull] IInvocationExpression invocation);
    }
}