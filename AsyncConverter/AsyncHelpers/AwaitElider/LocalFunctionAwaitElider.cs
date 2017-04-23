using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitElider
{
    [SolutionComponent]
    internal class LocalFunctionAwaitElider : ICustomAwaitElider
    {
        public bool CanElide(ICSharpDeclaration declarationOrClosure) => declarationOrClosure is ILocalFunctionDeclaration;

        public void Elide(ICSharpDeclaration declarationOrClosure, ICSharpExpression awaitExpression)
        {
            var factory = CSharpElementFactory.GetInstance(awaitExpression);

            var localFunctionDeclaration = declarationOrClosure as ILocalFunctionDeclaration;
            if (localFunctionDeclaration == null)
                return;

            localFunctionDeclaration.SetAsync(false);
            if (localFunctionDeclaration.Body != null)
            {
                var statement = factory.CreateStatement("return $0;", awaitExpression);
                awaitExpression.GetContainingStatement()?.ReplaceBy(statement);
            }
            else
                localFunctionDeclaration.ArrowClause?.SetExpression(awaitExpression);
        }
    }
}