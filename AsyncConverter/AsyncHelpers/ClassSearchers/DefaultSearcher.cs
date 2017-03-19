using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ClassSearchers
{
    [SolutionComponent]
    public class DefaultSearcher : IClassSearcher
    {
        public int Priority => 0;

        public ITypeElement GetClassForSearch(IParametersOwner originalMethod, IType invokedType)
        {
            return originalMethod.GetContainingType();
        }
    }
}