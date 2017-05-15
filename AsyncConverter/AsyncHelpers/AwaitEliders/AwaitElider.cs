using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

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
    }
}