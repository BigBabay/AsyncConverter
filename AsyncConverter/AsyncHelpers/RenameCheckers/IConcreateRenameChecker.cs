using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    public interface IConcreateRenameChecker
    {
        bool SkipRename([NotNull]IMethodDeclaration method);
    }
}