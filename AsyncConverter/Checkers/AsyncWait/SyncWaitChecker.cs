using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Checkers.AsyncWait
{
    [SolutionComponent]
    class SyncWaitChecker : ISyncWaitChecker
    {
        private readonly ISyncWaitConcreteChecker[] checkers;

        public SyncWaitChecker(IEnumerable<ISyncWaitConcreteChecker> checkers)
        {
            this.checkers = checkers.ToArray();
        }

        public bool CanReplaceWaitToAsync(IInvocationExpression invocationExpression)
            => checkers.All(checker => checker.CanReplaceWaitToAsync(invocationExpression));

        public bool CanReplaceResultToAsync(IReferenceExpression referenceExpression)
            => checkers.All(checker => checker.CanReplaceResultToAsync(referenceExpression));
    }
}