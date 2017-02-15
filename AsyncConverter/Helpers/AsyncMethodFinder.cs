using AsyncConverter.AsyncHelpers;
using AsyncConverter.AsyncHelpers.ClassSearchers;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Util;

namespace AsyncConverter.Helpers
{
    [SolutionComponent]
    public class AsyncMethodFinder : IAsyncMethodFinder
    {
        private readonly IParameterComparer parameterComparer;
        private readonly IClassForSearchResolver classForSearchResolver;

        public AsyncMethodFinder(IParameterComparer parameterComparer, IClassForSearchResolver classForSearchResolver)
        {
            this.parameterComparer = parameterComparer;
            this.classForSearchResolver = classForSearchResolver;
        }

        public IMethod FindEquivalentAsyncMethod(IParametersOwner originalMethod, IType invokedType)
        {
            if (!originalMethod.IsValid())
                return null;

            var @class = classForSearchResolver.GetClassForSearch(originalMethod, invokedType);
            if (@class == null)
                return null;

            var originalReturnType = originalMethod.Type();
            foreach (var candidateMethod in @class.Methods)
            {
                if (originalMethod.ShortName + "Async" != candidateMethod.ShortName)
                    continue;

                var returnType = candidateMethod.Type();
                if (returnType.IsTask() && !originalReturnType.IsVoid())
                    continue;

                if (!returnType.IsGenericTaskOf(originalReturnType))
                    continue;

                if (!parameterComparer.IsParametersEqual(candidateMethod.Parameters, originalMethod.Parameters))
                    continue;

                return candidateMethod;
            }
            return null;
        }
    }
}