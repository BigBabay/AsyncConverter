using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    public interface IMethodFindingChecker
    {
        [Pure]
        bool NeedSkip([NotNull] IMethod originalMethod, [NotNull] IMethod candidateMethod);
    }
}