using JetBrains.ReSharper.Psi;

namespace AsyncConverter.Helpers
{
    public interface IAsyncReplacer
    {
        void ReplaceToAsync(IMethod methodDeclaredElement);
    }
}