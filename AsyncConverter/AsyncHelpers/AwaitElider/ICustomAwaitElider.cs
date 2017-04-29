using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitElider
{
    public interface ICustomAwaitElider
    {
        bool CanElide(ICSharpDeclaration declarationOrClosure);
        void Elide(ICSharpDeclaration declarationOrClosure, ICSharpExpression awaitExpression);
    }
}