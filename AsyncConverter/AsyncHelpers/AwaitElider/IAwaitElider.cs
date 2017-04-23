using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers
{
    public interface IAwaitElider
    {
        void Elide(IAwaitExpression awaitExpression);
    }
}