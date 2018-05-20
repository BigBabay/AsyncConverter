using System.Collections.Generic;
using System.Linq;
using AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers.CustomCheckers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers
{
    [SolutionComponent]
    internal class ConfigureAwaitChecker : IConfigureAwaitChecker
    {
        private readonly IConfigureAwaitCustomChecker[] awaitCustomCheckers;

        public ConfigureAwaitChecker(IEnumerable<IConfigureAwaitCustomChecker> awaitCustomCheckers)
        {
            this.awaitCustomCheckers = awaitCustomCheckers.ToArray();
        }

        public bool NeedAdding(IAwaitExpression element)
        {
            return awaitCustomCheckers.All(x => x.NeedAdding(element));
        }
    }
}