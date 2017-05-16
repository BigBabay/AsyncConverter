using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    public interface IConcreteMethodFindingChecker
    {
        bool NeedSkip([NotNull] IMethod originalMethod, [NotNull] IMethod candidateMethod);
    }
}