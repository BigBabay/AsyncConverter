using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitEliders
{
    public interface IAwaitElider
    {
        void Elide(IAwaitExpression awaitExpression);
    }
}