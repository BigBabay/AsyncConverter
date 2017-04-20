using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;

namespace AsyncConverter.AsyncHelpers.AwaitElideChecker
{
    [SolutionComponent]
    internal class AwaitElideChecker : IAwaitElideChecker
    {
        public bool MayBeElided(IAwaitExpression element)
        {
            var statement = element.Parent as IExpressionStatement;
            if (statement != null
                && IsFinalStatement(statement)
                && statement.GetContainingNode<IUsingStatement>() == null
                && statement.GetContainingNode<ITryStatement>() == null)
                return true;

            var arrowExpressionClause = element.Parent as IArrowExpressionClause;
            if (arrowExpressionClause != null)
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