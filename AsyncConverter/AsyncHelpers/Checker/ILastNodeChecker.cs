using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.Checker
{
    public interface ILastNodeChecker
    {
        bool IsLastNode([NotNull] ICSharpExpression element);
    }
}