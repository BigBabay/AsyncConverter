using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IReturnStatement), HighlightingTypes = new[] { typeof(NullReturnAsTaskHighlighting)})]
    public class NullReturnFromMethodAnalyzer : ElementProblemAnalyzer<IReturnStatement>
    {
        protected override void Run(IReturnStatement element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var literalExpression = element.Value as ICSharpLiteralExpression;
            if(literalExpression?.Literal.GetTokenType() != CSharpTokenType.NULL_KEYWORD)
                return;

            var containingFunctionLikeDeclarationOrClosure = element.GetContainingFunctionLikeDeclarationOrClosure();
            var lambda = containingFunctionLikeDeclarationOrClosure as IAnonymousFunctionExpression;
            var method = containingFunctionLikeDeclarationOrClosure as IMethodDeclaration;
            if (lambda != null)
            {
                if (!lambda.ReturnType.IsTask() && !lambda.ReturnType.IsGenericTask())
                    return;
                if (lambda.IsAsync)
                    return;
                consumer.AddHighlighting(new NullReturnAsTaskHighlighting(literalExpression, lambda.ReturnType));
            }
            else if(method != null)
            {
                if (!method.Type.IsTask() && !method.Type.IsGenericTask())
                    return;
                if (method.IsAsync)
                    return;
                consumer.AddHighlighting(new NullReturnAsTaskHighlighting(literalExpression, method.Type));
            }
        }
    }

    [ElementProblemAnalyzer(typeof(ILambdaExpression), HighlightingTypes = new[] { typeof(NullReturnAsTaskHighlighting) })]
    public class NullReturnFromLambdaAnalyzer : ElementProblemAnalyzer<ILambdaExpression>
    {
        protected override void Run(ILambdaExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var literalExpression = element.BodyExpression as ICSharpLiteralExpression;
            if (literalExpression?.Literal.GetTokenType() != CSharpTokenType.NULL_KEYWORD)
                return; ;
            if(element.IsAsync)
                return;
            if (!element.ReturnType.IsTask() && !element.ReturnType.IsGenericTask())
                return;
            consumer.AddHighlighting(new NullReturnAsTaskHighlighting(literalExpression, element.ReturnType));
        }
    }
}