using System.Collections.Generic;
using System.Linq;
using AsyncConverter.Helpers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.AwaitEliders
{
    [SolutionComponent]
    public class AwaitEliderChecker : IAwaitEliderChecker
    {
        private readonly IConcreteAwaitEliderChecker[] checkers;

        public AwaitEliderChecker(IEnumerable<IConcreteAwaitEliderChecker> checkers)
        {
            this.checkers = checkers.ToArray();
        }

        public bool CanElide(IParametersOwnerDeclaration element) => checkers.All(x => x.CanElide(element));
    }
}