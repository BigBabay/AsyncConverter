using System.Linq;
using AsyncConverter.Settings;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace AsyncConverter.Checkers
{
    public interface IAttributeAssemblyChecker
    {
        bool IsUnder(ICSharpTreeNode node);
    }

    [SolutionComponent]
    public class AttributeAssemblyChecker : IAttributeAssemblyChecker
    {
        public bool IsUnder(ICSharpTreeNode node)
        {
            var store = node.GetSettingsStore();
            var customTypes = store.EnumIndexedValues(AsyncConverterSettingsAccessor.ConfigureAwaitIgnoreAttributeTypes).ToArray();

            if (customTypes.IsNullOrEmpty())
                return false;

            var containingFunctionLikeDeclarationOrClosure = node.;

            if (containingFunctionLikeDeclarationOrClosure?.ContainsAttribute(customTypes) == true)
            {
                return true;
            }

            return false;
        }
    }
}