using AsyncConverter.Checkers.AsyncWait;
using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] {typeof(AsyncWaitHighlighting)})]
    public class AsyncWaitAnalyzer : ElementProblemAnalyzer<IReferenceExpression>
    {
        private readonly ISyncWaitChecker syncWaitChecker;

        public AsyncWaitAnalyzer(ISyncWaitChecker syncWaitChecker)
        {
            this.syncWaitChecker = syncWaitChecker;
        }

        protected override void Run(IReferenceExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (syncWaitChecker.CanReplaceResultToAsync(element))
                consumer.AddHighlighting(new AsyncWaitHighlighting(element));
        }
    }
}