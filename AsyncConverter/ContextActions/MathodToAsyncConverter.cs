using System;
using AsyncConverter.Helpers;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AsyncConverter.ContextActions
{
    [ContextAction(Group = "C#", Name = "ConvertToAsync", Description = "Convert method to async and replace all inner call to async version if exist.")]
    public class MathodToAsyncConverter : ContextActionBase
    {
        private IAsyncReplacer asyncReplacer;
        private ICSharpContextActionDataProvider Provider { get; }

        public MathodToAsyncConverter(ICSharpContextActionDataProvider provider)
        {
            Provider = provider;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            asyncReplacer = solution.GetComponent<IAsyncReplacer>();

            var method = GetMethodFromCarretPosition();

            var methodDeclaredElement = method?.DeclaredElement;
            if (methodDeclaredElement == null)
                return null;

            asyncReplacer.ReplaceToAsync(methodDeclaredElement);
            return null;
        }

        public override string Text { get; } = "Convert method to async and replace all inner call to async version if exist.";
        public override bool IsAvailable(IUserDataHolder cache)
        {
            var method = GetMethodFromCarretPosition();
            if (method == null)
                return false;

            var returnType = method.DeclaredElement?.ReturnType;

            return returnType != null && !(returnType.IsTask() || returnType.IsGenericTask());
        }

        [CanBeNull]
        private IMethodDeclaration GetMethodFromCarretPosition()
        {
            var identifier = Provider.TokenAfterCaret as ICSharpIdentifier;
            identifier = identifier ?? Provider.TokenBeforeCaret as ICSharpIdentifier;
            return identifier?.Parent as IMethodDeclaration;
        }
    }
}