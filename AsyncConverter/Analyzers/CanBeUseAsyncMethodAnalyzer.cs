using AsyncConverter.AsyncHelpers.MethodFinders;
using AsyncConverter.Highlightings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Analyzers
{
    [ElementProblemAnalyzer(typeof(IInvocationExpression), HighlightingTypes = new[] {typeof(CanBeUseAsyncMethodHighlighting) })]
    public class CanBeUseAsyncMethodAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
    {
        private readonly IAsyncMethodFinder asyncMethodFinder;

        public CanBeUseAsyncMethodAnalyzer(IAsyncMethodFinder asyncMethodFinder)
        {
            this.asyncMethodFinder = asyncMethodFinder;
        }

        protected override void Run(IInvocationExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!element.IsUnderAsyncDeclaration())
                return;

            var referenceCurrentResolveResult = element.Reference?.Resolve();
            if (referenceCurrentResolveResult?.IsValid() != true)
                return;

            var invocationMethod = referenceCurrentResolveResult.DeclaredElement as IMethod;
            if (invocationMethod == null)
                return;

            var invokedType = (element.ConditionalQualifier as IReferenceExpression)?.QualifierExpression?.Type();

            var findingResult = asyncMethodFinder.FindEquivalentAsyncMethod(invocationMethod, invokedType);
            if (findingResult.Method == null || !findingResult.ParameterCompareResult.CanBeConvertedToAsync())
                return;

            consumer.AddHighlighting(new CanBeUseAsyncMethodHighlighting(element));
        }
    }
}