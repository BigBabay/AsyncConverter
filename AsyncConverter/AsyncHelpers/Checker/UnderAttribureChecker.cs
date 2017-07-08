using System.Linq;
using AsyncConverter.Settings;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AsyncConverter.AsyncHelpers.Checker
{
    [SolutionComponent]
    public class UnderAttributeChecker : IUnderAttributeChecker
    {
        public bool IsUnder(ICSharpTreeNode node)
        {
            var store = node.GetSettingsStore();
            var customTypeKey = store.EnumEntryIndices(AsyncConverterSettingsAccessor.ConfigureAwaitIgnoreAttributeTypes).ToArray();
            //var customAttributes = configureAwaitIgnoreAttributeTypes.Select(x => TypeFactory.CreateTypeByCLRName(x, node.GetPsiModule())).ToArray();

            if (customTypeKey.IsNullOrEmpty())
                return false;

            var containingFunctionLikeDeclarationOrClosure = node.GetContainingFunctionDeclarationIgnoringClosures() as IMethodDeclaration;

            if (containingFunctionLikeDeclarationOrClosure == null)
                return false;

            if (containingFunctionLikeDeclarationOrClosure
                .AttributeSectionList
                .AttributesEnumerable
                .Any(attribute => customTypeKey
                         .Any(customType => attribute.Name.QualifiedName == store.GetIndexedValue(AsyncConverterSettingsAccessor.ConfigureAwaitIgnoreAttributeTypes, customType))))
            {
                return false;
            }

            return true;
        }
    }
}
