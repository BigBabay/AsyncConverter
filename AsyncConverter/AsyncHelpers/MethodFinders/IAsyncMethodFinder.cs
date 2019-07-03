using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    public interface IAsyncMethodFinder
    {
        [NotNull]
        [Pure]
        FindingResult FindEquivalentAsyncMethod([NotNull] IMethod originalMethod, [CanBeNull] IType invokedType);
    }
}