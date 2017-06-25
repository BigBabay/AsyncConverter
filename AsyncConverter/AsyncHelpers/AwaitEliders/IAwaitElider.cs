using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitEliders
{
    public interface IAwaitElider
    {
        void Elide(IAwaitExpression awaitExpression);
        void Elide(IParametersOwnerDeclaration parametersOwnerDeclaration);
    }
}