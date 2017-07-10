using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.Checker
{
    public interface IAttributeFunctionChecker
    {
        bool IsUnder(ICSharpTreeNode node);

    }
}