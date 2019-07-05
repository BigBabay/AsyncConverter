using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.CanBeUseAsyncMethodCheckers
{
    [SolutionComponent]
    internal class InLockChecker : IConcreteCanBeUseAsyncMethodChecker
    {
        public bool CanReplace(IInvocationExpression element)
            => element.GetContainingNode<ILockStatement>() == null;
    }
}