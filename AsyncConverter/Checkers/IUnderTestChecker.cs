using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Checkers
{
    public interface IUnderTestChecker
    {
        bool IsUnder([NotNull]IMethodDeclaration method);
    }
}