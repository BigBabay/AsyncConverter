using AsyncConverter.Checkers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    [SolutionComponent]
    internal class TestRenameChecker : IConcreateRenameChecker
    {
        private readonly IUnderTestChecker underTestChecker;

        public TestRenameChecker(IUnderTestChecker underTestChecker)
        {
            this.underTestChecker = underTestChecker;
        }

        public bool SkipRename(IMethodDeclaration method) => underTestChecker.IsUnder(method);
    }
}