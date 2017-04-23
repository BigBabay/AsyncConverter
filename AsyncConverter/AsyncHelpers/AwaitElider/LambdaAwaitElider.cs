using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitElider
{
    [SolutionComponent]
    internal class LambdaAwaitElider : ICustomAwaitElider
    {
        public bool CanElide(ICSharpDeclaration declarationOrClosure) => declarationOrClosure is ILambdaExpression;

        public void Elide(ICSharpDeclaration declarationOrClosure, ICSharpExpression awaitExpression)
        {
            var factory = CSharpElementFactory.GetInstance(awaitExpression);

            var lambdaExpression = declarationOrClosure as ILambdaExpression;
            if (lambdaExpression == null)
                return;

            lambdaExpression.SetAsync(false);
            if (lambdaExpression.BodyBlock != null)
            {
                var statement = factory.CreateStatement("return $0;", awaitExpression);
                awaitExpression.GetContainingStatement()?.ReplaceBy(statement);
            }
            else
                lambdaExpression.SetBodyExpression(awaitExpression);
        }
    }
}