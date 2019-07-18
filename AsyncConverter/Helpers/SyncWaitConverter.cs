using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class SyncWaitConverter : ISyncWaitConverter
    {
        public void ReplaceWaitToAsync(IInvocationExpression invocationExpression)
        {
            var replaceBy = invocationExpression.FirstChild?.FirstChild;
            if (replaceBy != null)
                ReplaceToAwait(invocationExpression, replaceBy);
        }

        public void ReplaceResultToAsync(IReferenceExpression referenceExpression)
        {
            var replaceBy = referenceExpression.QualifierExpression;
            if (replaceBy != null)
                ReplaceToAwait(referenceExpression, replaceBy);
        }

        private static void ReplaceToAwait([NotNull] ICSharpExpression invocationExpression, [NotNull] ITreeNode replaceBy)
        {
            var factory = CSharpElementFactory.GetInstance(invocationExpression);
            var call = factory.CreateExpression("await $0.ConfigureAwait(false)", replaceBy);
            invocationExpression.ReplaceBy(call);
        }
    }
}