using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IAwaitExpression), HighlightingTypes = new[] {typeof(AsyncAwaitMayBeElidedHighlighting) })]
    public class AsyncAwaitMayBeElidedAnalyzer : ElementProblemAnalyzer<IAwaitExpression>
    {
        protected override void Run(IAwaitExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var expression = element.Parent as IExpressionStatement;
            if(expression == null)
                return;

            if (expression.GetNextStatement() != null)
                return;

            if(expression.GetContainingNode<IUsingStatement>() != null)
                return;

            if (expression.GetContainingNode<ITryStatement>() != null)
                return;

            consumer.AddHighlighting(new AsyncAwaitMayBeElidedHighlighting(element));
        }
    }
}