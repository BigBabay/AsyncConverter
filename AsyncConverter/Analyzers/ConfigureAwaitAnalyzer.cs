using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.CSharp.PostfixTemplates;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IAwaitExpression), HighlightingTypes = new[] {typeof(ConfigureAwaitHighlighting) })]
    public class ConfigureAwaitAnalyzer : ElementProblemAnalyzer<IAwaitExpression>
    {
        protected override void Run(IAwaitExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var declaredType = element.Task?.GetExpressionType() as IDeclaredType;
            if(!declaredType.IsConfigurableAwaitable() && !declaredType.IsGenericConfigurableAwaitable())
                consumer.AddHighlighting(new ConfigureAwaitHighlighting(element));
        }
    }
}