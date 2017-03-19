using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ClassSearchers
{
    [SolutionComponent]
    public class ClassForSearchResolver : IClassForSearchResolver
    {
        private readonly IClassSearcher[] classSearchers;

        public ClassForSearchResolver(IEnumerable<IClassSearcher> classSearchers)
        {
            this.classSearchers = classSearchers.OrderBy(x => x.Priority).ToArray();
        }

        public ITypeElement GetClassForSearch(IParametersOwner originalMethod, IType invokedType)
        {
            return classSearchers
                .Select(strategyResolver => strategyResolver.GetClassForSearch(originalMethod, invokedType))
                .FirstOrDefault(element => element != null);
        }
    }
}