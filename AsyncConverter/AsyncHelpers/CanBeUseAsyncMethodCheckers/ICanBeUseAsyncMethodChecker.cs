using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.CanBeUseAsyncMethodCheckers
{
    public interface ICanBeUseAsyncMethodChecker
    {
        bool CanReplace(IInvocationExpression element);
    }
}