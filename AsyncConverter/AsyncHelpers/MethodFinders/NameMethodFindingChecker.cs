using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    [SolutionComponent]
    public class NameMethodFindingChecker : IConcreteMethodFindingChecker
    {
        public bool NeedSkip(IMethod originalMethod, IMethod candidateMethod) => originalMethod.ShortName + "Async" != candidateMethod.ShortName;
    }
}