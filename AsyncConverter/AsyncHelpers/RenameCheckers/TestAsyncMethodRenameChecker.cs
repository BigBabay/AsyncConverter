using System.Collections.Generic;
using System.Linq;
using AsyncConverter.Settings;
using JetBrains.Application.Settings;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    [SolutionComponent]
    internal class TestRenameChecker : IConcreateRenameChecker
    {
        private readonly HashSet<ClrTypeName> testAttributesClass = new HashSet<ClrTypeName>
                                                           {
                                                               new ClrTypeName("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute"),
                                                               new ClrTypeName("Xunit.FactAttribute"),
                                                               new ClrTypeName("Xunit.TheoryAttribute"),
                                                               new ClrTypeName("NUnit.Framework.TestAttribute"),
                                                               new ClrTypeName("NUnit.Framework.TestCaseAttribute"),
                                                           };

        public bool SkipRename(IMethodDeclaration method)
        {
            if (method.AttributeSectionList == null)
                return false;

            var excludeTestMethods = method.GetSettingsStore().GetValue(AsyncSuffixSettingsAccessor.ExcludeTestMethodsFromAnalysis);
            if (!excludeTestMethods)
                return false;

            return method
                .AttributeSectionList
                .AttributesEnumerable
                .Select(attribute => attribute.Name.Reference.Resolve().DeclaredElement)
                .OfType<IClass>()
                .Select(attributeClass => attributeClass.GetClrName())
                .Any(clrTypeName => testAttributesClass.Contains(clrTypeName));
        }
    }
}