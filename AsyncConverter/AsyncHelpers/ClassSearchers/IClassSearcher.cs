using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ClassSearchers
{
    public interface IClassSearcher
    {
        int Priority { get; }

        [CanBeNull]
        [Pure]
        ITypeElement GetClassForSearch([NotNull] IParametersOwner originalMethod, [CanBeNull] IType invokedType);
    }
}