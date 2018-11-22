using AsyncConverter.Checkers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers.CustomCheckers
{
    [SolutionComponent]
    class AttributeTypeChecker : IConfigureAwaitCustomChecker
    {
        private readonly IAttributeTypeChecker attributeTypeChecker;

        public AttributeTypeChecker(IAttributeTypeChecker attributeTypeChecker)
        {
            this.attributeTypeChecker = attributeTypeChecker;
        }

        public bool CanBeAdded(IAwaitExpression element) => !attributeTypeChecker.IsUnder(element);
    }
}