using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    public interface IRenameChecker
    {
        bool NeedRename([NotNull] IMethodDeclaration method);
    }
}