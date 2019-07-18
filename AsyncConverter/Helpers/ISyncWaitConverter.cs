using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Helpers
{
    public interface ISyncWaitConverter
    {
        void ReplaceWaitToAsync([NotNull] IInvocationExpression invocationExpression);
        void ReplaceResultToAsync([NotNull] IReferenceExpression referenceExpression);
    }
}