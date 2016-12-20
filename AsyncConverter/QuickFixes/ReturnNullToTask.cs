using System;
using AsyncConverter.Highlightings;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AsyncConverter.QuickFixes
{
    [QuickFix]
    public class ReturnNullToTask : QuickFixBase
    {
        private readonly NullReturnAsTaskHighlighting error;

        public ReturnNullToTask(NullReturnAsTaskHighlighting error)
        {
            this.error = error;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var psiModule = error.CSharpLiteralExpression.GetPsiModule();
            var factory = CSharpElementFactory.GetInstance(psiModule);
            if (error.ReturnType.IsTask())
            {
                var type = TypeFactory.CreateTypeByCLRName("System.Threading.Tasks.Task", psiModule);
                var completedTask = factory.CreateReferenceExpression("$0.CompletedTask", type);
                error.CSharpLiteralExpression.ReplaceBy(completedTask);
            }
            else if (error.ReturnType.IsGenericTask())
            {
                var type = TypeFactory.CreateTypeByCLRName("System.Threading.Tasks.Task", psiModule);
                var wrappedNull = factory.CreateReferenceExpression("$0.FromResult(null)", type);
                error.CSharpLiteralExpression.ReplaceBy(wrappedNull);
            }
            return null;
        }

        public override string Text => "Wrap to Task";

        public override bool IsAvailable(IUserDataHolder cache)
        {
            return error.IsValid();
        }
    }
}
