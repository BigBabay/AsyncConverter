using AsyncConverter.Checkers;
using AsyncConverter.Helpers;
using AsyncConverter.Settings;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitEliders
{
    [SolutionComponent]
    public class EliderInTestChecker : IConcreteAwaitEliderChecker
    {
        private readonly IUnderTestChecker underTestChecker;

        public EliderInTestChecker(IUnderTestChecker underTestChecker)
        {
            this.underTestChecker = underTestChecker;
        }

        public bool CanElide(IParametersOwnerDeclaration element)
        {
            var excludeTestMethods = element.GetSettingsStore().GetValue(AsyncConverterSettingsAccessor.ExcludeTestMethodsFromEliding);
            if (!excludeTestMethods)
                return true;

            var method = element as IMethodDeclaration;

            if (method == null)
                return true;

            return !underTestChecker.IsUnder(method);
        }
    }
}