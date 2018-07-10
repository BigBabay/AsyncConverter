using System.Collections.Generic;
using AsyncConverter.Helpers;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.Checkers
{
    [SolutionComponent]
    public class UnderTestChecker : IUnderTestChecker
    {
        private readonly HashSet<ClrTypeName> testAttributesClass = new HashSet<ClrTypeName>
                                                                    {
                                                                        new ClrTypeName("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute"),
                                                                        new ClrTypeName("Microsoft.VisualStudio.TestTools.UnitTesting.DataTestMethodAttribute"),
                                                                        new ClrTypeName("Xunit.FactAttribute"),
                                                                        new ClrTypeName("Xunit.TheoryAttribute"),
                                                                        new ClrTypeName("NUnit.Framework.TestAttribute"),
                                                                        new ClrTypeName("NUnit.Framework.TestCaseAttribute"),
                                                                        new ClrTypeName("NUnit.Framework.TestCaseSourceAttribute"),
                                                                    };

        public bool IsUnder(IMethodDeclaration method) => method.AttributeSectionList != null && method.ContainsAttribute(testAttributesClass);
    }
}