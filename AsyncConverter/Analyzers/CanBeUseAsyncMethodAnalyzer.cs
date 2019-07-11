using AsyncConverter.AsyncHelpers.CanBeUseAsyncMethodCheckers;
using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IInvocationExpression), HighlightingTypes = new[] {typeof(CanBeUseAsyncMethodHighlighting) })]
    public class CanBeUseAsyncMethodAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
    {
        private readonly ICanBeUseAsyncMethodChecker canBeUseAsyncMethodChecker;

        public CanBeUseAsyncMethodAnalyzer(ICanBeUseAsyncMethodChecker canBeUseAsyncMethodChecker)
        {
            this.canBeUseAsyncMethodChecker = canBeUseAsyncMethodChecker;
        }

        protected override void Run(IInvocationExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if(!canBeUseAsyncMethodChecker.CanReplace(element))
                return;

            consumer.AddHighlighting(new CanBeUseAsyncMethodHighlighting(element));
        }
    }
}