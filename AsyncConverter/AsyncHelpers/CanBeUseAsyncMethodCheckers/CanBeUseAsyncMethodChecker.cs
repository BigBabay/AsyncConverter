using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.CanBeUseAsyncMethodCheckers
{
    [SolutionComponent]
    internal class CanBeUseAsyncMethodChecker : ICanBeUseAsyncMethodChecker
    {
        private readonly IConcreteCanBeUseAsyncMethodChecker[] checkers;

        public CanBeUseAsyncMethodChecker(IEnumerable<IConcreteCanBeUseAsyncMethodChecker> checkers)
        {
            this.checkers = checkers.ToArray();
        }

        public bool CanReplace(IInvocationExpression element)
        {
            return checkers.All(x => x.CanReplace(element));
        }
    }
}