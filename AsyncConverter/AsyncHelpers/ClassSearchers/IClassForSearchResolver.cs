using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ClassSearchers
{
    public interface IClassForSearchResolver
    {
        [Pure]
        [CanBeNull]
        ITypeElement GetClassForSearch([NotNull]IParametersOwner originalMethod, [CanBeNull] IType invokedType);
    }
}