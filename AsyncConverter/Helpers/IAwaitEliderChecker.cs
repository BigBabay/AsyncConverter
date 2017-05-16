using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Helpers
{
    public interface IAwaitEliderChecker
    {
        bool CanElide(IParametersOwnerDeclaration element);
    }
}