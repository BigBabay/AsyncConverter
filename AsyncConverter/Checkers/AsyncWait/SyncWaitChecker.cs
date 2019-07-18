using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Checkers.AsyncWait
{
    [SolutionComponent]
    class SyncWaitChecker : ISyncWaitChecker
    {
        public bool CanReplaceWaitToAsync(IInvocationExpression invocationExpression)
        {
            var shortName = invocationExpression.Reference?.Resolve().Result.DeclaredElement?.ShortName;
            return shortName == "Wait"
                || shortName == "AwaitResult";
        }

        public bool CanReplaceResultToAsync(IReferenceExpression referenceExpression)
        {
            var type = referenceExpression.QualifierExpression?.Type();
            return (type.IsGenericTask() || type.IsTask()) && IsResult(referenceExpression);
        }

        private bool IsResult([NotNull] IReferenceExpression referenceExpression)
        {
            return referenceExpression?.NameIdentifier?.Name == "Result";
        }
    }
}