using AsyncConverter.Helpers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Util;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    [SolutionComponent]
    public class ReturnTypeMethodFindingChecker : IConcreteMethodFindingChecker
    {
        public bool NeedSkip(IMethod originalMethod, IMethod candidateMethod)
        {
            var returnType = candidateMethod.Type();
            var originalReturnType = originalMethod.Type();
            return !returnType.IsTaskOf(originalReturnType);
        }
    }
}