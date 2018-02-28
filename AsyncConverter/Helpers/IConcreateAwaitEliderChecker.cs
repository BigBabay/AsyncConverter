using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Helpers
{
    public interface IConcreateAwaitEliderChecker
    {
        bool CanElide(IParametersOwnerDeclaration element);
    }
}