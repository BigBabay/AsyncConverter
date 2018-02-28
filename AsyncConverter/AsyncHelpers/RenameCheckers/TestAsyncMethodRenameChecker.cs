using AsyncConverter.Checkers;
using AsyncConverter.Settings;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

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

        public bool SkipRename(IMethodDeclaration method)
        {
            var excludeTestMethods = method.GetSettingsStore().GetValue(AsyncConverterSettingsAccessor.ExcludeTestMethodsFromRenaming);
            return excludeTestMethods
                   && underTestChecker.IsUnder(method);
        }
    }
}