using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    internal class AsyncInvocationReplacer : IAsyncInvocationReplacer
    {
        public void ReplaceInvocation(IInvocationExpression invocation, string newMethodName, bool useAwait)
        {
            var referenceExpression = invocation?.FirstChild as IReferenceExpression;
            if (referenceExpression?.NameIdentifier == null)
                return;

            var returnType = invocation.Type();

            var callFormat = useAwait
                ? "await $0($1).ConfigureAwait(false)"
                : returnType.IsVoid() ? "$0($1).Wait()" : "$0($1).Result";

            var factory = CSharpElementFactory.GetInstance(invocation);
            var newReferenceExpression = referenceExpression.QualifierExpression == null
                ? factory.CreateReferenceExpression("$0", newMethodName)
                : factory.CreateReferenceExpression("$0.$1", referenceExpression.QualifierExpression, newMethodName);

            newReferenceExpression.SetTypeArgumentList(referenceExpression.TypeArgumentList);

            var awaitExpression = factory.CreateExpression(callFormat, newReferenceExpression, invocation.ArgumentList);
            invocation.ReplaceBy(awaitExpression);
        }
    }
}