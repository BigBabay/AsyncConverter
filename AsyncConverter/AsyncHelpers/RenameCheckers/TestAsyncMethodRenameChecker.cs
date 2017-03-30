using System.Collections.Generic;
using System.Linq;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    [SolutionComponent]
    public class TestRenameChecker : IConcreateRenameChecker
    {
        private HashSet<ClrTypeName> testAttributesClass = new HashSet<ClrTypeName>
                                                           {
                                                               new ClrTypeName("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute"),
                                                               new ClrTypeName("Xunit.FactAttribute"),
                                                               new ClrTypeName("Xunit.TheoryAttribute"),
                                                               new ClrTypeName("NUnit.Framework.TestAttribute"),
                                                               new ClrTypeName("NUnit.Framework.TestCaseAttribute"),
                                                           };

        public bool NeedRename(IMethodDeclaration method)
        {
            if (method.AttributeSectionList == null)
                return true;

            foreach (var attribute in method.AttributeSectionList.AttributesEnumerable)
            {
                var attributeClass = attribute.Name.Reference.Resolve().DeclaredElement as IClass;
                if (attributeClass == null)
                    continue;

                var clrTypeName = attributeClass.GetClrName();
                if (testAttributesClass.Contains(clrTypeName))
                    return false;
            }
            return true;
        }
    }
}