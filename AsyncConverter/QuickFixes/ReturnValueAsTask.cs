using System;
using AsyncConverter.Helpers;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AsyncConverter.QuickFixes
{
    [QuickFix]
    public class ReturnValueAsTask : QuickFixBase
    {
        private readonly IncorrectReturnTypeError error;

        public ReturnValueAsTask(IncorrectReturnTypeError error)
        {
            this.error = error;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var expression = error.ReturnValueHolder.Value;

            if (expression == null)
                return null;

            var factory = CSharpElementFactory.GetInstance(expression);
            var cSharpArgument = factory.CreateExpression("Task.FromResult($0)", expression);
            expression.ReplaceBy(cSharpArgument);
            return null;
        }

        public override string Text => "Replace to Task.FromResult";

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var returnType = error.ReturnType;

            var valueType = error.ValueType.ToIType();

            return returnType.IsGenericTaskOf(valueType);
        }
    }
}