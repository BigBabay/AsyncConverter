using System.Linq;
using AsyncConverter.Helpers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;


namespace AsyncConverter.AsyncHelpers.AwaitEliders
{
    [SolutionComponent]
    internal class UsingVarChecker : IConcreteAwaitEliderChecker
    {
        public bool CanElide(IParametersOwnerDeclaration element)
        {
            var multipleLocalVariable = element.Descendants<IMultipleLocalVariableDeclaration>();
            return multipleLocalVariable.ToEnumerable().All(x => x.UsingKind == UsingDeclarationKind.Regular);
        }
    }
}