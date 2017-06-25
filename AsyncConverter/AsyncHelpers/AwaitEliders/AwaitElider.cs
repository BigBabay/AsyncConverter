using System.Collections.Generic;
using System.Linq;
using AsyncConverter.Helpers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitEliders
{
    [SolutionComponent]
    internal class AwaitElider : IAwaitElider
    {
        private readonly ICustomAwaitElider[] awaitEliders;

        public AwaitElider(IEnumerable<ICustomAwaitElider> awaitEliders)
        {
            this.awaitEliders = awaitEliders.ToArray();
        }

        public void Elide(IAwaitExpression awaitExpression)
        {
            var expression = awaitExpression.Task;
            var invocationExpression = expression as IInvocationExpression;

            var declarationOrClosure = awaitExpression.GetContainingFunctionLikeDeclarationOrClosure();

            var expressionWithoutConfigureAwait = invocationExpression.RemoveConfigureAwait();

            awaitEliders.FirstOrDefault(x => x.CanElide(declarationOrClosure))?.Elide(declarationOrClosure, expressionWithoutConfigureAwait);
        }

        public void Elide(IParametersOwnerDeclaration parametersOwnerDeclaration)
        {
            foreach (var awaitExpression in parametersOwnerDeclaration.DescendantsInScope<IAwaitExpression>())
            {
                Elide(awaitExpression);
            }
        }
    }
}