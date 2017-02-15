using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.Helpers
{
    public interface IAsyncMethodFinder
    {
        [CanBeNull]
        [Pure]
        IMethod FindEquivalentAsyncMethod([NotNull] IParametersOwner originalMethod, [CanBeNull] IType invokedType);
    }
}