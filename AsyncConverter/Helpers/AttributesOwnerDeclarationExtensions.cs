using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AsyncConverter.Helpers
{
    public static class AttributesOwnerDeclarationExtensions
    {
        public static bool ContainsAttribute([NotNull] this IAttributesOwnerDeclaration declaration, IEnumerable<ClrTypeName> attributeNames)
        {
            var clrTypeNames = new HashSet<ClrTypeName>(attributeNames);
            if (clrTypeNames.IsNullOrEmpty())
            {
                return false;
            }
            return declaration
                .AttributesEnumerable
                .Select(attribute => attribute.Name.Reference.Resolve().DeclaredElement)
                .OfType<IClass>()
                .Select(attributeClass => attributeClass.GetClrName())
                .Any(clrTypeNames.Contains);
        }

        public static bool ContainsAttribute([NotNull] this IAttributesOwnerDeclaration declaration, IEnumerable<string> attributeNames)
        {
            var clrTypeNames = new HashSet<ClrTypeName>(attributeNames.Select(x => new ClrTypeName(x)));
            if (clrTypeNames.IsNullOrEmpty())
            {
                return false;
            }
            return declaration
                .AttributesEnumerable
                .Select(attribute => attribute.Name.Reference.Resolve().DeclaredElement)
                .OfType<IClass>()
                .Select(attributeClass => attributeClass.GetClrName())
                .Any(clrTypeNames.Contains);
        }
    }
}