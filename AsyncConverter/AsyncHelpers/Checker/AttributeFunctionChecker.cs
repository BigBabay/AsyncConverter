using System.Linq;
using AsyncConverter.Helpers;
using AsyncConverter.Settings;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace AsyncConverter.AsyncHelpers.Checker
{
    [SolutionComponent]
    public class AttributeFunctionChecker : IAttributeFunctionChecker
    {
        public bool IsUnder(ICSharpTreeNode node)
        {
            var store = node.GetSettingsStore();
            var customTypes = store.EnumIndexedValues(AsyncConverterSettingsAccessor.ConfigureAwaitIgnoreAttributeTypes).ToArray();

            if (customTypes.IsNullOrEmpty())
                return false;

            var containingFunctionLikeDeclarationOrClosure = node.GetContainingFunctionDeclarationIgnoringClosures();

            if (containingFunctionLikeDeclarationOrClosure?.ContainsAttribute(customTypes) == true)
            {
                return true;
            }

            return false;
        }
    }
}
