using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Checkers
{
    public interface IAttributeTypeChecker
    {
        bool IsUnder(ICSharpTreeNode node);
    }
}