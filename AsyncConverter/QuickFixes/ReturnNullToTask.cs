using System;
using AsyncConverter.Highlightings;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AsyncConverter.QuickFixes
{
    [QuickFix]
    public class ReturnNullAsTask : QuickFixBase
    {
        private readonly NullReturnAsTaskHighlighting error;

        public ReturnNullAsTask(NullReturnAsTaskHighlighting error)
        {
            this.error = error;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var psiModule = error.CSharpLiteralExpression.GetPsiModule();
            var factory = CSharpElementFactory.GetInstance(psiModule);
            var taskType = TypeFactory.CreateTypeByCLRName("System.Threading.Tasks.Task", psiModule);

            if (error.ReturnType.IsTask())
            {
                var completedTask = factory.CreateReferenceExpression("$0.CompletedTask", taskType);
                error.CSharpLiteralExpression.ReplaceBy(completedTask);
            }
            else if (error.ReturnType.IsGenericTask())
            {
                var declaredReturnType = error.ReturnType as IDeclaredType;
                if (declaredReturnType == null)
                    return null;

                var substitution = declaredReturnType.GetSubstitution();
                var genericParameter = substitution.Apply(substitution.Domain[0]);
                var wrappedNull = factory.CreateReferenceExpression(
                    genericParameter.IsStructType() ? "$0.FromResult(default($1))" : "$0.FromResult<$1>(null)",
                    taskType,
                    genericParameter);
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
