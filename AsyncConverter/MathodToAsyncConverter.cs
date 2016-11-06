using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.Types;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.Psi.Xaml.Tree;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.ReSharper.SDK.Helper;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AsyncConverter
{
    [ContextAction(Group = "C#", Name = "ConvertToAsync", Description = "Convert method to async and replace all inner call to async version if exist.")]
    public class MathodToAsyncConverter : ContextActionBase
    {
        private ICSharpContextActionDataProvider Provider { get; }

        public MathodToAsyncConverter(ICSharpContextActionDataProvider provider)
        {
            Provider = provider;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var method = GetMethodFromCarretPosition();
            if (method == null)
                return null;

            var finder = Provider.PsiServices.Finder;
            var methodDeclaredElement = method.DeclaredElement;
            if (methodDeclaredElement == null)
                return null;

            var psiModule = method.GetPsiModule();
            var factory = CSharpElementFactory.GetInstance(psiModule);

            var usages = finder.FindReferences(methodDeclaredElement, SearchDomainFactory.Instance.CreateSearchDomain(psiModule), NullProgressIndicator.Instance);
            foreach (var usage in usages)
            {
                var invocation = usage.GetTreeNode().Parent as IInvocationExpression;
                var containingFunctionDeclarationIgnoringClosures = invocation?.InvokedExpression.GetContainingFunctionDeclarationIgnoringClosures();
                if (containingFunctionDeclarationIgnoringClosures == null)
                    continue;
                AsyncHelper.ReplaceCallToAsync(invocation, factory, containingFunctionDeclarationIgnoringClosures.IsAsync);
            }

            var invocationExpressions = method.Body.Descendants<IInvocationExpression>();
            foreach (var invocationExpression in invocationExpressions)
            {
                AsyncHelper.ReplaceToAsyncMethod(invocationExpression, factory);
            }

            AsyncHelper.ReplaceMethodSignatureToAsync(methodDeclaredElement, psiModule, method, finder);

            return null;
        }



        public override string Text { get; } = "Return empty collection";
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