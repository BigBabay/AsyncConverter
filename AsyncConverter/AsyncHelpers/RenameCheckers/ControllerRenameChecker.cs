using System.Linq;
using System.Collections.Generic;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    [SolutionComponent]
    internal class ControllerRenameChecker : IConcreateRenameChecker
    {
        private readonly HashSet<ClrTypeName> controllerClasses = new HashSet<ClrTypeName>
                                                           {
                                                               new ClrTypeName("System.Web.Mvc.Controller"),
                                                               new ClrTypeName("System.Web.Http.ApiController"),
                                                               new ClrTypeName("Microsoft.AspNetCore.Mvc.Controller"),
                                                               new ClrTypeName("Microsoft.AspNetCore.Mvc.ControllerBase"),
                                                           };

        public bool SkipRename(IMethodDeclaration method)
        {
            var @class = method.DeclaredElement?.GetContainingType() as IClass;
            if (@class == null)
                return false;
            var superTypes = @class.GetSuperTypesWithoutCircularDependent();
            if (superTypes.Any(superType => controllerClasses.Contains(superType.GetClrName())))
                return true;
            return false;
        }
    }
}