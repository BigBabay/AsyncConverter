using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Helpers
{
    public interface IConcreteAwaitEliderChecker
    {
        bool CanElide(IParametersOwnerDeclaration element);
    }
}