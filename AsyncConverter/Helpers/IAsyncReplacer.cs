using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.Helpers
{
    public interface IAsyncReplacer
    {
        void ReplaceToAsync([NotNull] IMethod methodDeclaredElement);
    }
}