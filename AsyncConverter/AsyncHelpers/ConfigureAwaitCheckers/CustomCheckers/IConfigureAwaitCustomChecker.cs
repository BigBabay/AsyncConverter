using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers.CustomCheckers
{
    public interface IConfigureAwaitCustomChecker
    {
        bool NeedAdding(IAwaitExpression element);
    }
}