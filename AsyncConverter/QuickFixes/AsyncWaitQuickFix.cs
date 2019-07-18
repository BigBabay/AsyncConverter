using System;
using AsyncConverter.Helpers;
using AsyncConverter.Highlightings;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AsyncConverter.QuickFixes
{
    [QuickFix]
    public class AsyncWaitQuickFix : QuickFixBase
    {
        private readonly AsyncWaitHighlighting asyncWaitHighlighting;

        public AsyncWaitQuickFix(AsyncWaitHighlighting asyncWaitHighlighting)
        {
            this.asyncWaitHighlighting = asyncWaitHighlighting;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var syncWaitConverter = solution.GetComponent<ISyncWaitConverter>();

            if(asyncWaitHighlighting.InvocationExpression != null)
                syncWaitConverter.ReplaceWaitToAsync(asyncWaitHighlighting.InvocationExpression);
            if(asyncWaitHighlighting.ReferenceExpression != null)
                syncWaitConverter.ReplaceResultToAsync(asyncWaitHighlighting.ReferenceExpression);

            return null;
        }

        public override string Text => "Use await";
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return asyncWaitHighlighting.IsValid();
        }
    }
}