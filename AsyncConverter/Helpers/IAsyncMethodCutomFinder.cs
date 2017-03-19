using AsyncConverter.AsyncHelpers;
using AsyncConverter.ParameterComparers;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Util;

namespace AsyncConverter.Helpers
{
    public interface IAsyncMethodCutomFinder
    {
        int Priority { get; }
        [CanBeNull]
        [Pure]
        IMethod FindEquivalentAsyncMethod([NotNull]IParametersOwner originalMethod, [CanBeNull] IType invokedType);
    }

    [SolutionComponent]
    public class AsyncMethodFinderWithConvertParamToFunc : IAsyncMethodCutomFinder
    {
        private readonly IParameterComparer parameterComparer;

        public AsyncMethodFinderWithConvertParamToFunc(IParameterComparer parameterComparer)
        {
            this.parameterComparer = parameterComparer;
        }

        public int Priority => 100;
        public IMethod FindEquivalentAsyncMethod(IParametersOwner originalMethod, IType invokedType)
        {
            if (!originalMethod.IsValid())
                return null;

            var originalReturnType = originalMethod.Type();

            var @class = originalMethod.GetContainingType();
            if (@class == null)
                return null;

            foreach (var candidateMethod in @class.Methods)
            {
                if (originalMethod.ShortName + "Async" != candidateMethod.ShortName)
                    continue;

                var returnType = candidateMethod.Type() as IDeclaredType;
                if (originalReturnType.IsVoid() && !returnType.IsTask()
                    || !originalReturnType.IsVoid() && !returnType.IsGenericTaskOf(originalReturnType))
                    continue;

                var parameterCompareResult = parameterComparer.ComparerParameters(originalMethod.Parameters, candidateMethod.Parameters);
                if(parameterCompareResult.Result != ParameterCompareAggregateResult.Equal && parameterCompareResult.Result != ParameterCompareAggregateResult.EqualOrCanBeConverting)
                    continue;

                return candidateMethod;
            }
            return null;
        }
    }
}