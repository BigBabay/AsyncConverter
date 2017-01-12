using System;
using System.Linq;
using AsyncConverter.Helpers;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
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

            var methodDeclaredElement = method?.DeclaredElement;
            if (methodDeclaredElement == null)
                return null;

            var finder = Provider.PsiServices.Finder;

            var psiModule = method.GetPsiModule();
            var factory = CSharpElementFactory.GetInstance(psiModule);

            foreach (var methodDeclaration in methodDeclaredElement
                .FindAllHierarchy()
                .SelectMany(x => x.GetDeclarations<IMethodDeclaration>()))
            {
                ReplaceMethodToAsync(finder, psiModule, factory, methodDeclaration);
            }
            return null;
        }

        private void ReplaceMethodToAsync(IFinder finder, IPsiModule psiModule, CSharpElementFactory factory, IMethodDeclaration method)
        {
            if(!method.IsValid())
                return;

            var methodDeclaredElement = method.DeclaredElement;
            if (methodDeclaredElement == null)
                return;

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
                AsyncHelper.TryReplaceInvocationToAsync(invocationExpression, factory, psiModule);
            }

            AsyncHelper.ReplaceMethodSignatureToAsync(methodDeclaredElement, psiModule, method);
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