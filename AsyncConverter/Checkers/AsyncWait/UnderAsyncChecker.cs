using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Checkers.AsyncWait
{
    [SolutionComponent]
    class UnderAsyncChecker : ISyncWaitConcreteChecker
    {
        public bool CanReplaceWaitToAsync(IInvocationExpression invocationExpression) => invocationExpression.IsUnderAsyncDeclaration();

        public bool CanReplaceResultToAsync(IReferenceExpression referenceExpression) => referenceExpression.IsUnderAsyncDeclaration();
    }
}