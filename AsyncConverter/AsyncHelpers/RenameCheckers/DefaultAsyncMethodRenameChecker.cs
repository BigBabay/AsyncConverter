using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    [SolutionComponent]
    public class DefaultRenameChecker : IConcreateRenameChecker
    {
        public bool SkipRename(IMethodDeclaration method)
        {
            if (!method.Type.IsTask() && !method.Type.IsGenericTask())
                return true;

            if (method.DeclaredName.EndsWith("Async"))
                return true;

            return false;
        }
    }
}