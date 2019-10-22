using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Checkers.AsyncWait
{
    internal interface ISyncWaitConcreteChecker
    {
        bool CanReplaceWaitToAsync(IInvocationExpression invocationExpression);

        bool CanReplaceResultToAsync(IReferenceExpression referenceExpression);
    }
}