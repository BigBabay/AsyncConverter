using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration), HighlightingTypes = new[] {typeof(AsyncMethodNamingHighlighting)})]
    public class AsyncMethodNamingAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if(!element.Type.IsTask() && !element.Type.IsGenericTask())
                return;

            if (element.DeclaredName.EndsWith("Async"))
                return;

            consumer.AddHighlighting(new AsyncMethodNamingHighlighting(element));
        }
    }
}