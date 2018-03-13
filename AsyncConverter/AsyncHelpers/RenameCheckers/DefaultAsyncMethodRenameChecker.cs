using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    [SolutionComponent]
    internal class DefaultRenameChecker : IConcreateRenameChecker
    {
        public bool SkipRename(IMethodDeclaration methodDeclaration)
        {
            if (!methodDeclaration.Type.IsTask() && !methodDeclaration.Type.IsGenericTask())
                return true;

            if (methodDeclaration.DeclaredName.EndsWith("Async"))
                return true;

            return false;
        }
    }
}