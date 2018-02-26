using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitEliders
{
    public interface IAwaitEliderChecker
    {
        bool CanElide(IParametersOwnerDeclaration element);
    }
}