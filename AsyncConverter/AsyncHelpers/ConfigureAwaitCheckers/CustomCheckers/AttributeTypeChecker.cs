using AsyncConverter.Checkers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers.CustomCheckers
{
    [SolutionComponent]
    class AttributeTypeChecker : IConfigureAwaitCustomChecker
    {
        public bool NeedAdding(IAwaitExpression element)
        {
            var attributeTypeChecker = element.GetSolution().GetComponent<IAttributeTypeChecker>();

            return !attributeTypeChecker.IsUnder(element);
        }
    }
}