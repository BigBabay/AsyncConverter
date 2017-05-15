using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitEliders
{
    [SolutionComponent]
    internal class MethodAwaitElider : ICustomAwaitElider
    {
        public bool CanElide(ICSharpDeclaration declarationOrClosure) => declarationOrClosure is IMethodDeclaration;

        public void Elide(ICSharpDeclaration declarationOrClosure, ICSharpExpression awaitExpression)
        {
            var factory = CSharpElementFactory.GetInstance(awaitExpression);

            var methodDeclaration = declarationOrClosure as IMethodDeclaration;
            if (methodDeclaration == null)
                return;

            methodDeclaration.SetAsync(false);
            if (methodDeclaration.Body != null)
            {
                var statement = factory.CreateStatement("return $0;", awaitExpression);
                awaitExpression.GetContainingStatement()?.ReplaceBy(statement);
            }
            else
                methodDeclaration.ArrowClause?.SetExpression(awaitExpression);
        }
    }
}