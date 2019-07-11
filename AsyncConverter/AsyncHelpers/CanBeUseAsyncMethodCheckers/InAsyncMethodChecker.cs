using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.CanBeUseAsyncMethodCheckers
{
    [SolutionComponent]
    internal class InAsyncMethodChecker : IConcreteCanBeUseAsyncMethodChecker
    {
        public bool CanReplace(IInvocationExpression element)
        {
            return element.IsUnderAsyncDeclaration();
        }
    }
}