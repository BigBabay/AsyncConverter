using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitElideChecker
{
    public interface IAwaitElideChecker
    {
        bool CanBeElided([NotNull] ITreeNode element);
    }
}