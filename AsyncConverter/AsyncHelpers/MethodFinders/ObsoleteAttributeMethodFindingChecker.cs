using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.MethodFinders
{
    [SolutionComponent]
    public class ObsoleteAttributeMethodFindingChecker : IConcreteMethodFindingChecker
    {
        private readonly ClrTypeName obsoleteClass = new ClrTypeName("System.ObsoleteAttribute");
        public bool NeedSkip(IMethod originalMethod, IMethod candidateMethod)
        {
            return !originalMethod.HasAttributeInstance(obsoleteClass, false) && candidateMethod.HasAttributeInstance(obsoleteClass, false);
        }
    }
}