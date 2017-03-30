using AsyncConverter.AsyncHelpers.RenameCheckers;
using AsyncConverter.Highlightings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration), HighlightingTypes = new[] {typeof(AsyncMethodNamingHighlighting)})]
    public class AsyncMethodNamingAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var checker = element.GetSolution().GetComponent<IRenameChecker>();

            if(!checker.NeedRename(element))
                return;

            consumer.AddHighlighting(new AsyncMethodNamingHighlighting(element));
        }
    }
}