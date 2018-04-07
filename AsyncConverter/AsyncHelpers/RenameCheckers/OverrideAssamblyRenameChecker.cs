using System.Linq;
using AsyncConverter.Helpers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    [SolutionComponent]
    public class OverrideAssamblyRenameChecker : IConcreateRenameChecker
    {
        public bool SkipRename(IMethodDeclaration methodDeclaration)
        {
            var method = methodDeclaration.DeclaredElement;
            if (method == null)
                return false;

            var baseMethods = method.FindBaseMethods();
            return baseMethods.Any(baseMethod => baseMethod.GetSourceFiles().IsEmpty);
        }
    }
}