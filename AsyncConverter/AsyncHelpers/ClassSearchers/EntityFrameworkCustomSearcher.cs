using AsyncConverter.Helpers;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace AsyncConverter.AsyncHelpers.ClassSearchers
{
    [SolutionComponent]
    public class EntityFrameworkCustomSearcher : IClassSearcher
    {
        public int Priority => -100;

        public ITypeElement GetClassForSearch(IParametersOwner originalMethod, IType invokedType)
        {
            var containingType = originalMethod.GetContainingType();
            if (containingType == null)
            {
                return null;
            }

            if (!invokedType.IsGenericIQueryable() || !containingType.IsEnumerableClass())
            {
                return null;
            }

            var queryableName = new ClrTypeName("System.Data.Entity.QueryableExtensions");
            var queryableType = TypeFactory.CreateTypeByCLRName(queryableName, NullableAnnotation.Unknown, invokedType.Module);
            var queryableTypeElement = queryableType.GetTypeElement();

            return queryableTypeElement;
        }
    }
}