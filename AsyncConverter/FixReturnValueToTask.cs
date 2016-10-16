using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Impl.Types;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AsyncConverter
{
    [QuickFix]
    public class FixReturnValueToTask : QuickFixBase
    {
        private readonly IncorrectArgumentTypeError error;
        private ICSharpTypeConversionRule cSharpTypeConversionRule;

        public FixReturnValueToTask(IncorrectArgumentTypeError error)
        {
            this.error = error;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var expression = error.Argument as ICSharpArgument;
            if (expression == null)
                return null;

            var file = expression.GetContainingFile() as ICSharpFile;
            if (file == null)
                return null;

            var psiModule = error.Reference.GetAccessContext().GetPsiModule();
            cSharpTypeConversionRule = expression.GetTypeConversionRule();
            var factory = CSharpElementFactory.GetInstance(psiModule);
            var cSharpArgument = factory.CreateArgument(ParameterKind.VALUE, factory.CreateExpression("Task.FromResult($0)", expression));
            expression.ReplaceBy(cSharpArgument);
            var taskUsing = factory.CreateUsingDirective("System.Threading.Tasks");
            if(file.ImportsEnumerable.OfType<IUsingSymbolDirective>().All(i => i.ImportedSymbolName.QualifiedName != "System.Threading.Tasks"))
                file.AddImport(taskUsing, true);
            return null;
        }

        public override string Text => "Replace to Task.FromResult";

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var parameterType = error.ParameterType;
            var parameterTypeClass = parameterType.GetClassType();

            if (parameterTypeClass == null || parameterTypeClass.GetClrName().FullName != "System.Threading.Tasks.Task`1")
                return false;

            var scalarType = parameterType.GetScalarType();
            if (scalarType == null)
                return false;

            var substitution = scalarType.GetSubstitution();
            if (substitution.IsEmpty())
                return false;

            var firstGenericParameterType = substitution.Apply(substitution.Domain[0]);

            var argumentType = error.ArgumentType.ToIType();
            if (argumentType == null)
                return false;

            return argumentType.IsSubtypeOf(firstGenericParameterType);
        }
    }
}
