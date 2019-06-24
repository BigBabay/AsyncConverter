using AsyncConverter.Helpers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AsyncConverter.AsyncHelpers.RenameCheckers
{
    [SolutionComponent]
    public class IsMainMethodChecker : IConcreateRenameChecker
    {
        public bool SkipRename(IMethodDeclaration methodDeclaration)
        {
            return methodDeclaration.DeclaredName == "Main"
                   && (methodDeclaration.Type.IsTask()
                       || (methodDeclaration.Type.IsGenericTask()
                           && methodDeclaration.Type.IsGenericTask()
                           && methodDeclaration.Type.GetFirstGenericType().IsInt())
                       )
                   && (methodDeclaration.ParameterDeclarations.Count == 0
                       || (methodDeclaration.ParameterDeclarations.Count == 1
                           && methodDeclaration.ParameterDeclarations[0].Type is IArrayType
                           && methodDeclaration.ParameterDeclarations[0].Type.GetScalarType().IsString())
                   );
        }
    }
}