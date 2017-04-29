using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitElideChecker
{
    public interface IAwaitElideChecker
    {
        bool MayBeElided([NotNull]IAwaitExpression element);
    }
}