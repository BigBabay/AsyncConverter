using AsyncConverter.AsyncHelpers.Checker;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers.CustomCheckers
{
    [SolutionComponent]
    internal class AttributeFunctionChecker: IConfigureAwaitCustomChecker
    {
        private readonly IAttributeFunctionChecker attributeFunctionChecker;

        public AttributeFunctionChecker(IAttributeFunctionChecker attributeFunctionChecker)
        {
            this.attributeFunctionChecker = attributeFunctionChecker;
        }

        public bool CanBeAdded(IAwaitExpression element)
        {
            return !attributeFunctionChecker.IsUnder(element);
        }
    }
}