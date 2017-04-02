using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Helpers
{
    public interface IAsyncReplacer
    {
        void ReplaceToAsync([NotNull] IMethod methodDeclaredElement);
        bool TryReplaceInvocationToAsync([NotNull] IInvocationExpression invocationExpression);
    }
}