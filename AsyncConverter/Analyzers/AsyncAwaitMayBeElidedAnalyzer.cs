using System.Linq;
using AsyncConverter.AsyncHelpers.AwaitElideChecker;
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
            var awaitElideChecker = element.GetSolution().GetComponent<IAwaitElideChecker>();

            var awaitExpressions = element.Descendants<IAwaitExpression>().ToEnumerable().ToArray();

            //TODO: think about this, different settings
            var expressionsInClosure = awaitExpressions.Where(x => x.GetContainingFunctionLikeDeclarationOrClosure() == element);
            if(expressionsInClosure.Count() != 1)
                return;

            var awaitExpression = awaitExpressions.First();
            if(!awaitElideChecker.MayBeElided(awaitExpression))
                return;

            consumer.AddHighlighting(new AsyncAwaitMayBeElidedHighlighting(awaitExpression));
        }
    }
}