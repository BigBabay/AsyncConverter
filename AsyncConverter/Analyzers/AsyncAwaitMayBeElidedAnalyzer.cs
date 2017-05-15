using System.Linq;
using AsyncConverter.AsyncHelpers.Checker;
using AsyncConverter.Helpers;
using AsyncConverter.Highlightings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IParametersOwnerDeclaration), HighlightingTypes = new[] {typeof(AsyncAwaitMayBeElidedHighlighting) })]
    public class AsyncAwaitMayBeElidedAnalyzer : ElementProblemAnalyzer<IParametersOwnerDeclaration>
    {
        protected override void Run(IParametersOwnerDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var awaitElideChecker = element.GetSolution().GetComponent<ILastNodeChecker>();

            var awaitExpressions = element.DescendantsInScope<IAwaitExpression>().ToArray();

            //TODO: think about this, different settings
            if(awaitExpressions.Length != 1)
                return;

            var awaitExpression = awaitExpressions.First();
            if(!awaitElideChecker.IsLastNode(awaitExpression))
                return;

            consumer.AddHighlighting(new AsyncAwaitMayBeElidedHighlighting(awaitExpression));
        }
    }
}