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
    public class CanBeUseAsyncMethodQuickFix : QuickFixBase
    {
        private readonly CanBeUseAsyncMethodHighlighting canBeUseAsyncMethodHighlighting;

        public CanBeUseAsyncMethodQuickFix(CanBeUseAsyncMethodHighlighting canBeUseAsyncMethodHighlighting)
        {
            this.canBeUseAsyncMethodHighlighting = canBeUseAsyncMethodHighlighting;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var asyncReplacer = solution.GetComponent<IAsyncReplacer>();

            asyncReplacer.TryReplaceInvocationToAsync(canBeUseAsyncMethodHighlighting.InvocationExpression);
            return null;
        }

        public override string Text => "Convert to async call";
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return canBeUseAsyncMethodHighlighting.IsValid();
        }
    }
}