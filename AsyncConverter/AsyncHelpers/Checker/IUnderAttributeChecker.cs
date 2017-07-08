using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.Checker
{
    public interface IUnderAttributeChecker
    {
        bool IsUnder(ICSharpTreeNode node);

    }
}