using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.Checker
{
    [SolutionComponent]
    internal class LastNodeChecker : ILastNodeChecker
    {
        public bool IsLastNode(ITreeNode element)
        {
            var statement = element.Parent as ICSharpStatement;
            if (statement != null
                && IsFinalStatement(statement)
                && statement.GetContainingNode<IUsingStatement>() == null
                && statement.GetContainingNode<ITryStatement>() == null)
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