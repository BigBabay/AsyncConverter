using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.Helpers
{
    public static class ParametersOwnerDeclarationExtensions
    {
        //TODO:alredy exists?
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<TNode> DescendantsInScope<TNode>([CanBeNull] this IParametersOwnerDeclaration root) where TNode : class, ICSharpTreeNode
        {
            if (root == null)
                return Enumerable.Empty<TNode>();

            var expressions = root.Descendants<TNode>().ToEnumerable();
            return expressions.Where(x => x.GetContainingFunctionLikeDeclarationOrClosure() == root);
        }
    }
}