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
            var identifier = Provider.TokenAfterCaret as ICSharpIdentifier;
            identifier = identifier ?? Provider.TokenBeforeCaret as ICSharpIdentifier;
            var method = identifier?.Parent as IMethodDeclaration;
            if (method == null)
                return null;

            var psiModule = method.GetPsiModule();
            var returnType = method.DeclaredElement?.ReturnType;

            if(returnType == null)
                return null;

            IDeclaredType newReturnValue;
            if (returnType.IsVoid())
            {
                newReturnValue = TypeFactory.CreateTypeByCLRName("System.Threading.Tasks.Task", psiModule);
            }
            else
            {
                var task = TypeFactory.CreateTypeByCLRName("System.Threading.Tasks.Task`1", psiModule).GetTypeElement();
                if (task == null)
                    return null;

                newReturnValue = TypeFactory.CreateType(task, returnType);
            }

            var factory = CSharpElementFactory.GetInstance(psiModule);

            var invocationExpressions = method.Body.Descendants<IInvocationExpression>();
            foreach (var invocationExpression in invocationExpressions)
            {
                ReplaceToAsyncMethod(invocationExpression, factory);
            }

            method.SetType(newReturnValue);
            method.SetAsync(true);
            CSharpImplUtil.ReplaceIdentifier(method.NameIdentifier, $"{method.NameIdentifier.Name}Async");

            return null;
        }

        private void ReplaceToAsyncMethod(IInvocationExpression invocationExpression, CSharpElementFactory factory)
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
            var referenceName = factory.CreateReferenceName(asyncMethod.ShortName);

            foreach (var child in referenceExpression.Children())
            {
                var invocationChild = child as IInvocationExpression;
                if (invocationChild != null)
                {
                    ReplaceToAsyncMethod(invocationChild, factory);
                }
            }

            var referenceExpressionLastChild = referenceExpression.LastChild as IIdentifier;
            if (referenceExpressionLastChild == null)
                return;
            //don't understand why not work, think later
            //referenceExpression.SetNameIdentifier(referenceName.NameIdentifier);
            using (WriteLockCookie.Create(true))
            {

                ModificationUtil.ReplaceChild<IIdentifier>(referenceExpressionLastChild, referenceName.NameIdentifier);
            }
            var awaitExpression = factory.CreateExpression("await $0($1).ConfigureAwait(false)", referenceExpression,
                invocationExpression.ArgumentList);
            invocationExpression.ReplaceBy(awaitExpression);
        }

        [CanBeNull]
        private IMethod FindEquivalentAsyncMethod([NotNull]IMethod originalMethod)
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

        private void AddTaskNamespace(CSharpElementFactory factory)
        {
            var usingDirective = factory.CreateUsingDirective("System.Threading.Tasks");
            var reference = usingDirective.ImportedSymbolName;
            var importScope = CSharpReferenceBindingUtil.GetImportScope(reference.Reference);
            var taskNamespace = reference.Reference.Resolve().DeclaredElement as INamespace;

            if(taskNamespace == null)
                return;

            if (!UsingUtil.CheckNamespaceAlreadyImported(importScope, taskNamespace))
            {
                UsingUtil.AddImportTo(importScope, usingDirective);
            }
        }

        private bool IsParameterEquals(IList<IParameter> methodParameters, IList<IParameter> originalParameters)
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
            var identifier = Provider.TokenAfterCaret as ICSharpIdentifier;
            identifier = identifier ?? Provider.TokenBeforeCaret as ICSharpIdentifier;
            var method = identifier?.Parent as IMethodDeclaration;
            if (method == null)
                return false;

            var returnType = method.DeclaredElement?.ReturnType;

            return returnType != null && !(returnType.IsTask() || returnType.IsGenericTask());
        }
    }
}