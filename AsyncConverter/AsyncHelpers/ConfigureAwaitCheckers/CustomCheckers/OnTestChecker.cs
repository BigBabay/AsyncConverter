using AsyncConverter.Checkers;
using AsyncConverter.Settings;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers.CustomCheckers
{
    [SolutionComponent]
    internal class OnTestChecker : IConfigureAwaitCustomChecker
    {
        private readonly IUnderTestChecker underTestChecker;

        public OnTestChecker(IUnderTestChecker underTestChecker)
        {
            this.underTestChecker = underTestChecker;
        }

        public bool CanBeAdded(IAwaitExpression element)
        {
            var excludeTestMethods = element.GetSettingsStore().GetValue(AsyncConverterSettingsAccessor.ExcludeTestMethodsFromConfigureAwait);
            var methodDeclaration =
                element.GetContainingTypeMemberDeclarationIgnoringClosures() as IMethodDeclaration;
            if (methodDeclaration == null)
                return true;
            return !excludeTestMethods || !underTestChecker.IsUnder(methodDeclaration);
        }
    }
}