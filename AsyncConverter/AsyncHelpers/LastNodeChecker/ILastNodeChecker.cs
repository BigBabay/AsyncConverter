using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.LastNodeChecker
{
    public interface ILastNodeChecker
    {
        bool IsLastNode([NotNull] ITreeNode element);
    }
}