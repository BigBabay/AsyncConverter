using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.CanBeUseAsyncMethodCheckers
{
    public interface IConcreteCanBeUseAsyncMethodChecker
    {
        bool CanReplace(IInvocationExpression element);
    }
}