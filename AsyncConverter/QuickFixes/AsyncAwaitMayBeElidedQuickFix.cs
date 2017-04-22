using System;
using AsyncConverter.Highlightings;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AsyncConverter.QuickFixes
{
    [QuickFix]
    public class AsyncAwaitMayBeElidedQuickFix : QuickFixBase
    {
        private readonly AsyncAwaitMayBeElidedHighlighting asyncAwaitMayBeElidedHighlighting;

        public AsyncAwaitMayBeElidedQuickFix(AsyncAwaitMayBeElidedHighlighting asyncAwaitMayBeElidedHighlighting)
        {
            this.asyncAwaitMayBeElidedHighlighting = asyncAwaitMayBeElidedHighlighting;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var awaitExpression = asyncAwaitMayBeElidedHighlighting.AwaitExpression;
            var factory = CSharpElementFactory.GetInstance(awaitExpression);

            var expression = awaitExpression.Task;
            var invocationExpression = expression as IInvocationExpression;

            var declarationOrClosure = awaitExpression.GetContainingFunctionLikeDeclarationOrClosure();
            //(declarationOrClosure as IAnonymousFunctionExpression)?.SetAsync(false);

            ICSharpExpression expressionWithoutConfigureAwait;
            if ((invocationExpression?.Reference?.Resolve().Result.DeclaredElement as IMethod)?.XMLDocId == "M:System.Threading.Tasks.Task.ConfigureAwait(System.Boolean)")
            {
                expressionWithoutConfigureAwait = (invocationExpression.FirstChild as IReferenceExpression)?.QualifierExpression;
                if (expressionWithoutConfigureAwait == null)
                    return null;
            }
            else
            {
                expressionWithoutConfigureAwait = expression;
            }

            var methodDeclaration = declarationOrClosure as IMethodDeclaration;
            if (methodDeclaration != null)
            {
                methodDeclaration.SetAsync(false);
                if (methodDeclaration.Body != null)
                {
                    var statement = factory.CreateStatement("return $0;", expressionWithoutConfigureAwait);
                    awaitExpression.GetContainingStatement()?.ReplaceBy(statement);
                }
                else
                    methodDeclaration.ArrowClause?.SetExpression(expressionWithoutConfigureAwait);
            }
            }
            return null;
        }

        public override string Text => "Remove async/await.";
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return asyncAwaitMayBeElidedHighlighting.IsValid();
        }
    }
}