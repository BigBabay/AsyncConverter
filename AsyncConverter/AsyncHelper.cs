using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;

namespace AsyncConverter
{
    public static class AsyncHelper
    {
        [CanBeNull]
        [Pure]
        public static IMethod FindEquivalentAsyncMethod([NotNull]IParametersOwner originalMethod)
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

                var returnType = candidateMethod.Type();
                if (returnType.IsTask() && !originalReturnType.IsVoid())
                    continue;

                if(!returnType.IsGenericTaskOf(originalReturnType))
                    continue;

                if(!IsParameterEquals(candidateMethod.Parameters, originalMethod.Parameters))
                    continue;

                return candidateMethod;
            }
            return null;
        }

        public static void TryReplaceInvocationToAsync([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory)
        {
            if (!invocationExpression.IsValid())
                return;
            foreach (var argument in invocationExpression.Arguments)
            {
                var argumentInvocationExpression = argument.Expression as IInvocationExpression;
                if (argumentInvocationExpression != null)
                    TryReplaceInvocationToAsync(argumentInvocationExpression, factory);
            }

            if (invocationExpression.Type().IsGenericTask() || invocationExpression.Type().IsTask())
            {
                ConvertCallWithWaitToAwaitCall(invocationExpression, factory);
            }
            else
            {
                TryConvertSyncCallToAsyncCall(invocationExpression, factory);
            }
        }

        private static void TryConvertSyncCallToAsyncCall(IInvocationExpression invocationExpression, CSharpElementFactory factory)
        {
            var referenceCurrentResolveResult = invocationExpression.Reference?.Resolve();
            if (referenceCurrentResolveResult?.IsValid() != true)
                return;

            var invocationMethod = referenceCurrentResolveResult.DeclaredElement as IMethod;
            if (invocationMethod == null)
                return;

            var asyncMethod = FindEquivalentAsyncMethod(invocationMethod);
            if (asyncMethod != null)
            {
                var referenceExpression = invocationExpression.FirstChild as IReferenceExpression;
                if (referenceExpression == null)
                    return;

                foreach (var child in referenceExpression.Children())
                {
                    var invocationChild = child as IInvocationExpression;
                    if (invocationChild != null)
                    {
                        TryReplaceInvocationToAsync(invocationChild, factory);
                    }
                }

                ReplaceCallToAsync(invocationExpression, factory, useAsync: true);
            }
            else
            {
                var asyncMethodWithFunc = FindEquivalentAsyncMethodWithAsyncFunc(invocationMethod);
                if (asyncMethodWithFunc != null)
                {
                    var referenceExpression = invocationExpression.FirstChild as IReferenceExpression;
                    if (referenceExpression == null)
                        return;

                    foreach (var child in referenceExpression.Children())
                    {
                        var invocationChild = child as IInvocationExpression;
                        if (invocationChild != null)
                        {
                            TryReplaceInvocationToAsync(invocationChild, factory);
                        }
                    }
                    if (TryConvertParameterFuncToAsync(invocationExpression, factory))
                    {
                        ReplaceCallToAsync(invocationExpression, factory, useAsync: true);
                    }
                }
            }
        }

        private static bool TryConvertParameterFuncToAsync(IInvocationExpression invocationExpression, CSharpElementFactory factory)
        {
            var lambdaExpressions = invocationExpression.Arguments.SelectNotNull(x => x.Value as ILambdaExpression);
            foreach (var functionExpression in lambdaExpressions)
            {
                //var invocationExpressions = functionExpression.BodyBlock.Descendants<IInvocationExpression>();
                //foreach (var innerInvocationExpression in invocationExpressions)
                //{
                //    ReplaceToAsyncMethod(innerInvocationExpression, factory);
                //}

                var innerInvocationExpression = functionExpression.BodyExpression as IInvocationExpression;
                if (innerInvocationExpression == null)
                    return false;
                functionExpression.SetAsync(true);
                TryReplaceInvocationToAsync(innerInvocationExpression, factory);
            }
            return true;
        }

        [Pure]
        [CanBeNull]
        private static IMethod FindEquivalentAsyncMethodWithAsyncFunc([NotNull]IMethod originalMethod)
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
                if((originalReturnType.IsVoid() && !returnType.IsTask())
                    ||(!originalReturnType.IsVoid() && !returnType.IsGenericTaskOf(originalReturnType)))
                    continue;

                if (!IsParameterEqualsOrAsyncFunc(originalMethod.Parameters, candidateMethod.Parameters))
                    continue;

                return candidateMethod;
            }
            return null;
        }

        private static void ConvertCallWithWaitToAwaitCall([NotNull] IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory)
        {
            var reference = invocationExpression.Parent as IReferenceExpression;

            //TODO: AwaitResult our custom method extension, it must be moded to settings
            if (reference?.NameIdentifier.Name == "AwaitResult" || reference?.NameIdentifier.Name == "Wait")
            {
                var call = factory.CreateExpression("await $0($1).ConfigureAwait(false)", invocationExpression.ConditionalQualifier,
                    invocationExpression.ArgumentList);
                var parentInvocation = reference.Parent as IInvocationExpression;
                if (parentInvocation == null)
                    return;
                parentInvocation.ReplaceBy(call);
            }

            if (reference?.NameIdentifier.Name == "Result")
            {
                var call = factory.CreateExpression("await $0($1).ConfigureAwait(false)", invocationExpression.ConditionalQualifier,
                    invocationExpression.ArgumentList);
                reference.ReplaceBy(call);
            }
        }

        public static void ReplaceMethodSignatureToAsync(IParametersOwner methodDeclaredElement, IPsiModule psiModule, IMethodDeclaration methodDeclaration)
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

        private static void SetSignature(IMethodDeclaration methodDeclaration, IType newReturnValue, string newName)
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
            if (referenceExpression == null)
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

        private static string GenerateAsyncMethodName(string oldName)
        {
            return oldName.EndsWith("Async") ? oldName : $"{oldName}Async";
        }

        private static bool IsParameterEqualsOrAsyncFunc(IList<IParameter> originalParameters, IList<IParameter> methodParameters)
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

        private static bool IsAsyncDelegate(IParameter originalParameter, IParameter parameter)
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

        private static bool IsParameterEquals(IList<IParameter> originalParameters, IList<IParameter> methodParameters)
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

        public static List<FindResultOverridableMember> FindImplementingMembers([NotNull] IOverridableMember overridableMember, [NotNull] IProgressIndicator pi)
        {
            var found = new List<FindResultOverridableMember>();
            overridableMember.GetPsiServices()
                .AsyncFinder.FindImplementingMembers(overridableMember, overridableMember.GetSearchDomain(), new FindResultConsumer(result =>
                {
                    var overridableMember1 = result as FindResultOverridableMember;
                    if (overridableMember1 != null)
                        found.Add(overridableMember1);
                    return FindExecution.Continue;
                }), true, pi);
            return found;
        }
    }
}