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
                ReplaceCallToAsync(invocation, factory, containingFunctionDeclarationIgnoringClosures.IsAsync);
            }

            var invocationExpressions = method.Body.Descendants<IInvocationExpression>();
            foreach (var invocationExpression in invocationExpressions)
            {
                ReplaceToAsyncMethod(invocationExpression, factory);
            }

            ReplaceMethodSignatureToAsync(methodDeclaredElement, psiModule, method, finder);

            return null;
        }

        private static void ReplaceMethodSignatureToAsync(IParametersOwner methodDeclaredElement, IPsiModule psiModule, IMethodDeclaration methodDeclaration, IFinder finder)
        {
            var returnType = methodDeclaredElement.ReturnType;

            IDeclaredType newReturnValue;
            if (returnType.IsVoid())
            {
                newReturnValue = TypeFactory.CreateTypeByCLRName("System.Threading.Tasks.Task", psiModule);
            }
            else
            {
                var task = TypeFactory.CreateTypeByCLRName("System.Threading.Tasks.Task`1", psiModule).GetTypeElement();
                if (task == null)
                    return;
                newReturnValue = TypeFactory.CreateType(task, returnType);
            }

            var newName = $"{methodDeclaration.DeclaredName}Async";

            foreach (var method in finder.FindImmediateBaseElements(methodDeclaration.DeclaredElement, NullProgressIndicator.Instance))
            {
                var baseMethodDeclarations = method.GetDeclarations();
                foreach (var declaration in baseMethodDeclarations.OfType<IMethodDeclaration>())
                {
                    SetSignature(declaration, newReturnValue, newName);
                }
            }

            SetSignature(methodDeclaration, newReturnValue, newName);

        }

        private static void SetSignature(IMethodDeclaration methodDeclaration, IDeclaredType newReturnValue, string newName)
        {
            methodDeclaration.SetType(newReturnValue);
            if(!methodDeclaration.IsAbstract)
                methodDeclaration.SetAsync(true);
            methodDeclaration.SetName(newName);
        }

        private static void ReplaceToAsyncMethod([NotNull]IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory)
        {
            if (!invocationExpression.IsValid())
                return;
            foreach (var argument in invocationExpression.Arguments)
            {
                var argumentInvocationExpression = argument.Expression as IInvocationExpression;
                if (argumentInvocationExpression != null)
                    ReplaceToAsyncMethod(argumentInvocationExpression, factory);
            }
            var referenceCurrentResolveResult = invocationExpression.Reference?.Resolve();
            if (referenceCurrentResolveResult?.IsValid() != true)
                return;
            var invocationMethod = referenceCurrentResolveResult.DeclaredElement as IMethod;
            if (invocationMethod == null)
                return;
            var asyncMethod = FindEquivalentAsyncMethod(invocationMethod);
            if (asyncMethod == null)
                return;

            var referenceExpression = invocationExpression.FirstChild as IReferenceExpression;
            if (referenceExpression == null)
                return;

            foreach (var child in referenceExpression.Children())
            {
                var invocationChild = child as IInvocationExpression;
                if (invocationChild != null)
                {
                    ReplaceToAsyncMethod(invocationChild, factory);
                }
            }

            ReplaceCallToAsync(invocationExpression, factory, true);
        }

        private static void ReplaceCallToAsync([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory, bool useAsync)
        {
            var returnType = invocationExpression.Type();
            var referenceExpression = invocationExpression.FirstChild as IReferenceExpression;
            if (referenceExpression == null)
                return;

            var newMethodName = $"{referenceExpression.NameIdentifier.Name}Async";

            var newReferenceExpression = referenceExpression.QualifierExpression == null
                ? factory.CreateReferenceExpression("$0", newMethodName)
                : factory.CreateReferenceExpression("$0.$1", referenceExpression.QualifierExpression, newMethodName);

            var callFormat = useAsync
                ? "await $0($1).ConfigureAwait(false)"
                : returnType.IsVoid() ? "$0($1).Wait()" : "$0($1).Result";
            var awaitExpression = factory.CreateExpression(callFormat, newReferenceExpression, invocationExpression.ArgumentList);
            invocationExpression.ReplaceBy(awaitExpression);
        }

        [CanBeNull]
        private static IMethod FindEquivalentAsyncMethod([NotNull]IParametersOwner originalMethod)
        {
            if (!originalMethod.IsValid())
                return null;

            var originalReturnType = originalMethod.Type();

            var @class = originalMethod.GetContainingType();
            if (@class == null)
                return null;

            foreach (var candidateMethod in @class.Methods)
            {
                var returnType = candidateMethod.Type() as IDeclaredType;
                if(!returnType.IsTask() && !returnType.IsGenericTask())
                    continue;

                if(originalMethod.ShortName + "Async" != candidateMethod.ShortName)
                    continue;

                var substitution = returnType.GetSubstitution();
                if (substitution.IsEmpty())
                    continue;

                var firstGenericParameterType = substitution.Apply(substitution.Domain[0]);
                if(!firstGenericParameterType.Equals(originalReturnType))
                    continue;

                if(!IsParameterEquals(candidateMethod.Parameters, originalMethod.Parameters))
                    continue;

                return candidateMethod;
            }
            return null;
        }

        private static bool IsParameterEquals(IList<IParameter> methodParameters, IList<IParameter> originalParameters)
        {
            if (methodParameters.Count != originalParameters.Count)
                return false;

            for (var i = 0; i < methodParameters.Count; i++)
            {
                var parameter = methodParameters[i];
                var originalParameter = originalParameters[i];
                if(!parameter.Type.Equals(originalParameter.Type))
                    return false;
            }
            return true;
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