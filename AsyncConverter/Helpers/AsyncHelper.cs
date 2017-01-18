using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;

namespace AsyncConverter.Helpers
{
    public static class AsyncHelper
    {
        public static bool TryReplaceInvocationToAsync([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory, [NotNull] IPsiModule psiModule)
        {
            if (!invocationExpression.IsValid())
                return false;
            foreach (var argument in invocationExpression.Arguments)
            {
                var argumentInvocationExpression = argument.Expression as IInvocationExpression;
                if (argumentInvocationExpression != null)
                    TryReplaceInvocationToAsync(argumentInvocationExpression, factory, psiModule);
            }

            if (invocationExpression.Type().IsGenericTask() || invocationExpression.Type().IsTask())
            {
                return ConvertCallWithWaitToAwaitCall(invocationExpression, factory);
            }

            return TryConvertSyncCallToAsyncCall(invocationExpression, factory, psiModule);
        }

        private static bool TryConvertSyncCallToAsyncCall([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory, [NotNull] IPsiModule psiModule)
        {
            var referenceCurrentResolveResult = invocationExpression.Reference?.Resolve();
            if (referenceCurrentResolveResult?.IsValid() != true)
                return false;

            var invocationMethod = referenceCurrentResolveResult.DeclaredElement as IMethod;
            if (invocationMethod == null)
                return false;

            var invokedObjectType = (invocationExpression.ConditionalQualifier as IReferenceExpression)?.QualifierExpression?.Type();

            var asyncMethod = FindEquivalentAsyncMethod(invocationMethod, invokedObjectType, psiModule);
            if (asyncMethod != null)
            {
                if (!TryConvertInnerReferenceToAsync(invocationExpression, factory, psiModule))
                    return false;
                ReplaceCallToAsync(invocationExpression, factory, useAsync: true);
                return true;
            }

            var asyncMethodWithFunc = FindEquivalentAsyncMethodWithAsyncFunc(invocationMethod);
            if (asyncMethodWithFunc == null)
                return false;
            if (!TryConvertInnerReferenceToAsync(invocationExpression, factory, psiModule))
                return false;

            if (TryConvertParameterFuncToAsync(invocationExpression, factory, psiModule))
                ReplaceCallToAsync(invocationExpression, factory, useAsync: true);
            return true;
        }

        private static bool TryConvertInnerReferenceToAsync([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory, [NotNull] IPsiModule psiModule)
        {
            var referenceExpression = invocationExpression.FirstChild as IReferenceExpression;
            if (referenceExpression == null)
                return false;

            foreach (var child in referenceExpression.Children())
            {
                var invocationChild = child as IInvocationExpression;
                if (invocationChild != null)
                    TryReplaceInvocationToAsync(invocationChild, factory, psiModule);
            }
            return true;
        }

        private static bool TryConvertParameterFuncToAsync([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory, [NotNull] IPsiModule psiModule)
        {
            var lambdaExpressions = invocationExpression.Arguments.SelectNotNull(x => x.Value as ILambdaExpression);
            invocationExpression.PsiModule.GetPsiServices().Transactions.StartTransaction("convertAsyncParameter");
            foreach (var functionExpression in lambdaExpressions)
            {
                functionExpression.SetAsync(true);
                var innerInvocationExpressions = functionExpression.Descendants<IInvocationExpression>();
                foreach (var innerInvocationExpression in innerInvocationExpressions)
                {
                    if (!TryReplaceInvocationToAsync(innerInvocationExpression, factory, psiModule))
                    {
                        invocationExpression.PsiModule.GetPsiServices().Transactions.RollbackTransaction();
                        return false;
                    }
                }
            }
            invocationExpression.PsiModule.GetPsiServices().Transactions.CommitTransaction();
            return true;
        }

        [Pure]
        [CanBeNull]
        private static IMethod FindEquivalentAsyncMethodWithAsyncFunc([NotNull] IMethod originalMethod)
        {
            if (!originalMethod.IsValid())
                return null;

            var originalReturnType = originalMethod.Type();

            var @class = originalMethod.GetContainingType();
            if (@class == null)
                return null;

            foreach (var candidateMethod in @class.Methods)
            {
                if (originalMethod.ShortName + "Async" != candidateMethod.ShortName)
                    continue;

                var returnType = candidateMethod.Type() as IDeclaredType;
                if ((originalReturnType.IsVoid() && !returnType.IsTask())
                    || (!originalReturnType.IsVoid() && !returnType.IsGenericTaskOf(originalReturnType)))
                    continue;

                if (!IsParameterEqualsOrAsyncFunc(originalMethod.Parameters, candidateMethod.Parameters))
                    continue;

                return candidateMethod;
            }
            return null;
        }

        private static bool ConvertCallWithWaitToAwaitCall([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory)
        {
            var reference = invocationExpression.Parent as IReferenceExpression;

            //TODO: AwaitResult our custom method extension, it must be moved to settings
            if (reference?.NameIdentifier?.Name == "AwaitResult" || reference?.NameIdentifier?.Name == "Wait")
            {
                var call = factory.CreateExpression("await $0($1).ConfigureAwait(false)", invocationExpression.ConditionalQualifier,
                    invocationExpression.ArgumentList);
                var parentInvocation = reference.Parent as IInvocationExpression;
                if (parentInvocation == null)
                    return false;
                parentInvocation.ReplaceBy(call);
                return true;
            }

            if (reference?.NameIdentifier?.Name == "Result")
            {
                var call = factory.CreateExpression("await $0($1).ConfigureAwait(false)", invocationExpression.ConditionalQualifier,
                    invocationExpression.ArgumentList);
                reference.ReplaceBy(call);
                return true;
            }
            return false;
        }

        public static void ReplaceMethodSignatureToAsync([NotNull] IParametersOwner methodDeclaredElement, [NotNull] IPsiModule psiModule, [NotNull] IMethodDeclaration methodDeclaration)
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

            var name = GenerateAsyncMethodName(methodDeclaration.DeclaredName);

            SetSignature(methodDeclaration, newReturnValue, name);
        }

        private static void SetSignature([NotNull] IMethodDeclaration methodDeclaration, [NotNull] IType newReturnValue, [NotNull] string newName)
        {
            methodDeclaration.SetType(newReturnValue);
            if (!methodDeclaration.IsAbstract)
                methodDeclaration.SetAsync(true);
            methodDeclaration.SetName(newName);
        }

        public static void ReplaceCallToAsync([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory, bool useAsync)
        {
            var returnType = invocationExpression.Type();
            var referenceExpression = invocationExpression.FirstChild as IReferenceExpression;
            if (referenceExpression?.NameIdentifier == null)
                return;

            var newMethodName = GenerateAsyncMethodName(referenceExpression.NameIdentifier.Name);

            var newReferenceExpression = referenceExpression.QualifierExpression == null
                ? factory.CreateReferenceExpression("$0", newMethodName)
                : factory.CreateReferenceExpression("$0.$1", referenceExpression.QualifierExpression, newMethodName);

            newReferenceExpression.SetTypeArgumentList(referenceExpression.TypeArgumentList);

            var callFormat = useAsync
                ? "await $0($1).ConfigureAwait(false)"
                : returnType.IsVoid() ? "$0($1).Wait()" : "$0($1).Result";
            var awaitExpression = factory.CreateExpression(callFormat, newReferenceExpression, invocationExpression.ArgumentList);
            invocationExpression.ReplaceBy(awaitExpression);
        }

        private static string GenerateAsyncMethodName([NotNull] string oldName)
        {
            return oldName.EndsWith("Async") ? oldName : $"{oldName}Async";
        }

        private static bool IsParameterEqualsOrAsyncFunc([NotNull, ItemNotNull] IList<IParameter> originalParameters, [NotNull, ItemNotNull] IList<IParameter> methodParameters)
        {
            if (methodParameters.Count != originalParameters.Count)
                return false;

            for (var i = 0; i < methodParameters.Count; i++)
            {
                var parameter = methodParameters[i];
                var originalParameter = originalParameters[i];
                if (!parameter.Type.Equals(originalParameter.Type)
                    && !IsAsyncDelegate(originalParameter, parameter))
                    return false;
            }
            return true;
        }

        private static bool IsAsyncDelegate([NotNull] IParameter originalParameter, [NotNull] IParameter parameter)
        {
            if (originalParameter.Type.IsAction() && parameter.Type.IsFunc())
            {
                var parameterDeclaredType = parameter.Type as IDeclaredType;
                var substitution = parameterDeclaredType?.GetSubstitution();
                if (substitution?.Domain.Count != 1)
                    return false;

                var valuableType = substitution.Apply(substitution.Domain[0]);
                return valuableType.IsTask();
            }
            if (originalParameter.Type.IsFunc() && parameter.Type.IsFunc())
            {
                var parameterDeclaredType = parameter.Type as IDeclaredType;
                var originalParameterDeclaredType = originalParameter.Type as IDeclaredType;
                var substitution = parameterDeclaredType?.GetSubstitution();
                var originalSubstitution = originalParameterDeclaredType?.GetSubstitution();
                if (substitution == null || substitution.Domain.Count != originalSubstitution?.Domain.Count)
                    return false;

                var i = 0;
                for (; i < substitution.Domain.Count - 1; i++)
                {
                    var genericType = substitution.Apply(substitution.Domain[i]);
                    var originalGenericType = originalSubstitution.Apply(originalSubstitution.Domain[i]);
                    if (!genericType.Equals(originalGenericType))
                        return false;
                }
                var returnType = substitution.Apply(substitution.Domain[i]);
                var originalReturnType = originalSubstitution.Apply(originalSubstitution.Domain[i]);
                return returnType.IsGenericTaskOf(originalReturnType);
            }

            return false;
        }

        [CanBeNull]
        [Pure]
        public static IMethod FindEquivalentAsyncMethod([NotNull] IParametersOwner originalMethod, [CanBeNull] IType invokedObjectType, [NotNull] IPsiModule psiModule)
        {
            if (!originalMethod.IsValid())
                return null;

            var @class = GetClassForMethodSearch(invokedObjectType, originalMethod, psiModule);
            if (@class == null)
                return null;

            var originalReturnType = originalMethod.Type();
            foreach (var candidateMethod in @class.Methods)
            {
                if (originalMethod.ShortName + "Async" != candidateMethod.ShortName)
                    continue;

                var returnType = candidateMethod.Type();
                if (returnType.IsTask() && !originalReturnType.IsVoid())
                    continue;

                if (!returnType.IsGenericTaskOf(originalReturnType))
                    continue;

                if (!IsParameterEquals(candidateMethod.Parameters, originalMethod.Parameters))
                    continue;

                return candidateMethod;
            }
            return null;
        }

        [CanBeNull]
        [Pure]
        private static ITypeElement GetClassForMethodSearch(IType invokedObjectType, [NotNull] IParametersOwner originalMethod, [NotNull] IPsiModule psiModule)
        {
            var containingType = originalMethod.GetContainingType();
            if (containingType == null)
            {
                return null;
            }

            if (!invokedObjectType.IsGenericIQueryable() || !IsEnumerable(containingType))
            {
                return containingType;
            }

            var queryableName = new ClrTypeName("System.Data.Entity.QueryableExtensions");
            var queryableType = TypeFactory.CreateTypeByCLRName(queryableName, psiModule);
            var queryableTypeElement = queryableType.GetTypeElement();

            return queryableTypeElement;
        }

        private static bool IsEnumerable(ITypeElement type)
        {
            var typeName = type.GetClrName();
            return typeName.Equals(PredefinedType.ENUMERABLE_CLASS);
        }

        private static bool IsParameterEquals([ItemNotNull] IList<IParameter> originalParameters, [ItemNotNull] IList<IParameter> methodParameters)
        {
            if (methodParameters.Count != originalParameters.Count)
                return false;

            for (var i = 0; i < methodParameters.Count; i++)
            {
                var parameter = methodParameters[i];
                var originalParameter = originalParameters[i];

                if (!parameter.Type.IsEquals(originalParameter.Type))
                    return false;
            }
            return true;
        }
    }
}