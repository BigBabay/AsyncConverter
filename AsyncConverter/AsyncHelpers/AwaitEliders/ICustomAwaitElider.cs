using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitEliders
{
    public interface ICustomAwaitElider
    {
        bool CanElide(ICSharpDeclaration declarationOrClosure);
        void Elide(ICSharpDeclaration declarationOrClosure, ICSharpExpression awaitExpression);
    }
}