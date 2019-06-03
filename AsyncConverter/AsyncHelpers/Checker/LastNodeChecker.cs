using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;

namespace AsyncConverter.AsyncHelpers.Checker
{
    [SolutionComponent]
    internal class LastNodeChecker : ILastNodeChecker
    {
        public bool IsLastNode(ICSharpExpression element)
        {
            var parentStatement = element.Parent as ICSharpStatement;
            if ((parentStatement is IReturnStatement || parentStatement is IExpressionStatement)
                && IsFinalStatement(parentStatement)
                && parentStatement.GetContainingNode<IUsingStatement>() == null
                && parentStatement.GetContainingNode<ITryStatement>() == null
                && parentStatement.GetContainingNode<ILoopStatement>() == null
                && parentStatement.GetContainingNode<IIfStatement>() == null
                )
                return true;

            var arrowExpressionClause = element.Parent as IArrowExpressionClause;
            if (arrowExpressionClause != null)
                return true;

            var lambdaExpression = element.Parent as ILambdaExpression;
            if (lambdaExpression != null)
                return true;

            return false;

        }

        private bool IsFinalStatement([NotNull]ICSharpStatement statement)
        {
            while (statement.GetNextStatement() == null)
            {
                if (statement == null)
                {
                    return true;
                }
                statement = statement.GetContainingStatement();
            }
            return false;
        }
    }
}