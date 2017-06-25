using System;
using AsyncConverter.AsyncHelpers.AwaitEliders;
using AsyncConverter.Highlightings;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
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
            var awaitElider = solution.GetComponent<IAwaitElider>();

            var awaitExpression = asyncAwaitMayBeElidedHighlighting.AwaitExpression;
            awaitElider.Elide(awaitExpression);

            return null;
        }

        public override string Text => "Remove async/await.";
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return asyncAwaitMayBeElidedHighlighting.IsValid();
        }
    }
}