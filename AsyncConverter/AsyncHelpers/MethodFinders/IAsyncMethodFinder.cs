using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    public interface IAsyncMethodFinder
    {
        [NotNull]
        [Pure]
        FindingReslt FindEquivalentAsyncMethod([NotNull] IMethod originalMethod, [CanBeNull] IType invokedType);
    }
}