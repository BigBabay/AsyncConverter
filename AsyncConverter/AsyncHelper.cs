using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

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

        public static void ReplaceToAsyncMethod([NotNull]IInvocationExpression invocationExpression, [NotNull] CSharpElementFactory factory)
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

        public static void ReplaceMethodSignatureToAsync(IParametersOwner methodDeclaredElement, IPsiModule psiModule, IMethodDeclaration methodDeclaration, IFinder finder)
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

        private static bool IsParameterEquals(IList<IParameter> methodParameters, IList<IParameter> originalParameters)
        {
            if (methodParameters.Count != originalParameters.Count)
                return false;

            for (var i = 0; i < methodParameters.Count; i++)
            {
                var parameter = methodParameters[i];
                var originalParameter = originalParameters[i];
                if (!parameter.Type.Equals(originalParameter.Type))
                    return false;
            }
            return true;
        }
    }
}