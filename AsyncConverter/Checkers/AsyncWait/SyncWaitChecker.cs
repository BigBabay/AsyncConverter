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
            var targetType = ResolveTargetType(invocationExpression);
            return (shortName == "Wait" || shortName == "AwaitResult") && targetType != null && (targetType.IsTask() || targetType.IsGenericTask());
        }

        private IType ResolveTargetType(IInvocationExpression invocationExpression)
        {
            return (invocationExpression.InvokedExpression.FirstChild as IReferenceExpression)?.Type()
                   ?? (invocationExpression.InvokedExpression.FirstChild as IInvocationExpression)?.Type();
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