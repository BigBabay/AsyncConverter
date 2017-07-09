using AsyncConverter.AsyncHelpers.Checker;
using AsyncConverter.Checkers;
using AsyncConverter.Highlightings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.CSharp.PostfixTemplates;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IAwaitExpression), HighlightingTypes = new[] {typeof(ConfigureAwaitHighlighting) })]
    public class ConfigureAwaitAnalyzer : ElementProblemAnalyzer<IAwaitExpression>
    {
        protected override void Run(IAwaitExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var attributeFunctionChecker = element.GetSolution().GetComponent<IAttributeFunctionChecker>();
            var attributeTypeChecker = element.GetSolution().GetComponent<IAttributeTypeChecker>();

            var declaredType = element.Task?.GetExpressionType() as IDeclaredType;

            if (declaredType.IsConfigurableAwaitable() || declaredType.IsGenericConfigurableAwaitable())
                return;

            if (attributeFunctionChecker.IsUnder(element))
                return;

            if (attributeTypeChecker.IsUnder(element))
                return;

            consumer.AddHighlighting(new ConfigureAwaitHighlighting(element));
        }
    }
}