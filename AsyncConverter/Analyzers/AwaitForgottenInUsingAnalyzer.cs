using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IUsingStatement), HighlightingTypes = new[] {typeof(AwaitForgottenInUsingHighlighting) })]
    public class AwaitForgottenInUsingAnalyzer : ElementProblemAnalyzer<IUsingStatement>
    {
        protected override void Run(IUsingStatement element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            //var awaitElideChecker = element.GetSolution().GetComponent<IAwaitElideChecker>();
            if (element.Declaration.IsVar)
            {
                var variableDeclaration = element.Declaration.Declarators.FirstOrDefault();
                if(variableDeclaration == null)
                    return;
                var variableDeclarationType = variableDeclaration.Type;
            }

            //consumer.AddHighlighting(new AsyncAwaitMayBeElidedHighlighting(awaitExpression));
        }
    }
}