using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Helpers
{
    public interface IInvocationConverter
    {
        bool TryReplaceInvocationToAsync([NotNull] IInvocationExpression invocationExpression);
    }
}