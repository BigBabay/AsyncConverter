using System.Linq;
using AsyncConverter.AsyncHelpers;
using AsyncConverter.AsyncHelpers.Checker;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class MainAwaitEliderChecker : IConcreateAwaitEliderChecker
    {
        private readonly ILastNodeChecker lastNodeChecker;

        public MainAwaitEliderChecker(ILastNodeChecker lastNodeChecker)
        {
            this.lastNodeChecker = lastNodeChecker;
        }

        public bool CanElide(IParametersOwnerDeclaration element)
        {
            var returnStatements = element.DescendantsInScope<IReturnStatement>().ToArray();
            var returnType = element.DeclaredParametersOwner?.ReturnType;
            if (returnType == null)
                return false;

            if (returnType.IsTask() && returnStatements.Any()
                || returnType.IsGenericTask() && returnStatements.Length > 1)
                return false;

            var awaitExpressions = element.DescendantsInScope<IAwaitExpression>().ToArray();

            //TODO: think about this, different settings
            if (awaitExpressions.Length != 1)
                return false;

            var awaitExpression = awaitExpressions.First();

            if (returnStatements.Any() && returnStatements.First() != awaitExpression.GetContainingStatement())
                return false;

            if (!lastNodeChecker.IsLastNode(awaitExpression))
                return false;

            var awaitingType = (awaitExpression.Task as IInvocationExpression)?.RemoveConfigureAwait()?.Type();
            if (awaitingType == null)
                return false;

            if (!awaitingType.Equals(returnType) && !(returnType.IsTask() && awaitingType.IsGenericTask()) && !returnType.IsVoid())
                return false;

            return true;
        }
    }
}