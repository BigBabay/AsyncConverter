using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers
{
    public interface IConfigureAwaitChecker
    {
        bool NeedAdding(IAwaitExpression element);
    }
}