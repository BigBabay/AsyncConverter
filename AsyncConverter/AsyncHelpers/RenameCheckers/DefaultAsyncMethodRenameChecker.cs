using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    [SolutionComponent]
    public class DefaultRenameChecker : IConcreateRenameChecker
    {
        public bool NeedRename(IMethodDeclaration method)
        {
            if (!method.Type.IsTask() && !method.Type.IsGenericTask())
                return false;

            if (method.DeclaredName.EndsWith("Async"))
                return false;

            return true;
        }
    }
}