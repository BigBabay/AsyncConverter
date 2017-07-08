using System.Linq;
using AsyncConverter.Helpers;
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
            var customTypes = store.EnumIndexedValues(AsyncConverterSettingsAccessor.ConfigureAwaitIgnoreAttributeTypes).ToArray();

            if (customTypes.IsNullOrEmpty())
                return false;

            var containingFunctionLikeDeclarationOrClosure = node.GetContainingFunctionDeclarationIgnoringClosures() as IMethodDeclaration;

            if (containingFunctionLikeDeclarationOrClosure?.ContainsAttribute(customTypes) == true)
            {
                return true;
            }

            return false;
        }
    }
}
