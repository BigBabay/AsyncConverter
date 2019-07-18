using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Checkers.AsyncWait
{
    public interface ISyncWaitChecker
    {
        bool CanReplaceWaitToAsync([NotNull] IInvocationExpression invocationExpression);
        bool CanReplaceResultToAsync([NotNull] IReferenceExpression referenceExpression);
    }
}