using System;
using AsyncConverter.Highlightings;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Feature.Services.Refactorings.Specific.Rename;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AsyncConverter.QuickFixes
{
    [QuickFix]
    public class AsyncMethodNamingQuickFix : QuickFixBase
    {
        private readonly AsyncMethodNamingHighlighting asyncMethodNamingHighlighting;

        public AsyncMethodNamingQuickFix(AsyncMethodNamingHighlighting asyncMethodNamingHighlighting)
        {
            this.asyncMethodNamingHighlighting = asyncMethodNamingHighlighting;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var methodDeclaration = asyncMethodNamingHighlighting.MethodDeclaration.DeclaredElement;
            if (methodDeclaration == null)
                return null;

            RenameRefactoringService.Rename(solution, new RenameDataProvider(methodDeclaration, methodDeclaration.ShortName + "Async"));

            return null;
        }

        public override string Text => "Add \"Async\" suffix";
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return asyncMethodNamingHighlighting.IsValid();
        }
    }
}