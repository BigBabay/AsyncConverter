using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    [SolutionComponent]
    public class MethodFindingChecker : IMethodFindingChecker
    {
        private readonly IConcreteMethodFindingChecker[] concreteMethodFindingCheckers;

        public MethodFindingChecker(IEnumerable<IConcreteMethodFindingChecker> concreteMethodFindingCheckers)
        {
            this.concreteMethodFindingCheckers = concreteMethodFindingCheckers.ToArray();
        }

        public bool NeedSkip(IMethod originalMethod, IMethod candidateMethod)
        {
            return concreteMethodFindingCheckers.Any(x => x.NeedSkip(originalMethod, candidateMethod));
        }
    }
}