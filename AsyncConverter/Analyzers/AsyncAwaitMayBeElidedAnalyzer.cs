using AsyncConverter.Helpers;
using AsyncConverter.Highlightings;
using JetBrains.ProjectModel;
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
            var awaitElideChecker = element.GetSolution().GetComponent<IAwaitEliderChecker>();

            if (awaitElideChecker.CanElide(element))
            {
                foreach (var awaitExpression in element.DescendantsInScope<IAwaitExpression>())
                {
                    consumer.AddHighlighting(new AsyncAwaitMayBeElidedHighlighting(awaitExpression));
                }
            }
        }
    }
}