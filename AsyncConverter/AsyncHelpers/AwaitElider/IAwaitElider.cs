using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitElider
{
    public interface IAwaitElider
    {
        void Elide(IAwaitExpression awaitExpression);
    }
}