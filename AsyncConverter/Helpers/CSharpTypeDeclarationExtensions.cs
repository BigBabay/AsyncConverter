using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AsyncConverter.Helpers
{
    public static class CSharpTypeDeclarationExtensions
    {
        public static bool ContainsAttribute([NotNull] this ICSharpTypeDeclaration declaration, IEnumerable<ClrTypeName> attributeNames)
        {
            var clrTypeNames = attributeNames.ToHashSet();
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

        public static bool ContainsAttribute([NotNull] this ICSharpTypeDeclaration declaration, IEnumerable<string> attributeNames)
        {
            var clrTypeNames = attributeNames.Select(x => new ClrTypeName(x)).ToHashSet();
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
