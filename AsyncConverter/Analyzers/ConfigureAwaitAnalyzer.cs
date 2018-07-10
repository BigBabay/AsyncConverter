using AsyncConverter.AsyncHelpers.ConfigureAwaitCheckers;
using AsyncConverter.Highlightings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IAwaitExpression), HighlightingTypes = new[] {typeof(ConfigureAwaitHighlighting) })]
    public class ConfigureAwaitAnalyzer : ElementProblemAnalyzer<IAwaitExpression>
    {
        protected override void Run(IAwaitExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var needConfAwaitCheckers = element.GetSolution().GetComponent<IConfigureAwaitChecker>();

            if(!needConfAwaitCheckers.NeedAdding(element))
                return;

            consumer.AddHighlighting(new ConfigureAwaitHighlighting(element));
        }
    }
}