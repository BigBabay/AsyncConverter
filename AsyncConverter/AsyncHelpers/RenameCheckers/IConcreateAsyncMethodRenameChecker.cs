using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    public interface IConcreateRenameChecker
    {
        bool NeedRename([NotNull]IMethodDeclaration method);
    }
}