using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.PostfixTemplates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers.CustomCheckers
{
    [SolutionComponent]
    internal class BaseChecker : IConfigureAwaitCustomChecker
    {
        public bool NeedAdding(IAwaitExpression element)
        {
            var declaredType = element.Task?.GetExpressionType() as IDeclaredType;

            return !declaredType.IsConfigurableAwaitable() && !declaredType.IsGenericConfigurableAwaitable();
        }
    }
}