using AsyncConverter.Checkers.AsyncWait;
using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IInvocationExpression), HighlightingTypes = new[] {typeof(AsyncWaitHighlighting)})]
    public class AsyncResultAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
    {
        private readonly ISyncWaitChecker syncWaitChecker;

        public AsyncResultAnalyzer(ISyncWaitChecker syncWaitChecker)
        {
            this.syncWaitChecker = syncWaitChecker;
        }

        protected override void Run(IInvocationExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (syncWaitChecker.CanReplaceWaitToAsync(element))
                consumer.AddHighlighting(new AsyncWaitHighlighting(element));
        }
    }
}