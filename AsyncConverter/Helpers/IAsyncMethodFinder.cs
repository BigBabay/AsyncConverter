using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.Helpers
{
    public interface IAsyncMethodFinder
    {
        [NotNull]
        [Pure]
        FindingReslt FindEquivalentAsyncMethod([NotNull] IParametersOwner originalMethod, [CanBeNull] IType invokedType);
    }
}