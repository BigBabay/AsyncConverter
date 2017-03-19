using System;
using System.Linq;
using AsyncConverter.AsyncHelpers.ParameterComparers;
using JetBrains.ProjectModel;

namespace AsyncConverter.AsyncHelpers
{
    [SolutionComponent]
    public class ParameterCompareResolver : IParameterCompareResolver
    {
        public ParameterCompareAggregateResult Resolve(CompareResult[] parameterResults)
        {
            return parameterResults.Aggregate(ParameterCompareAggregateResult.Equal, Aggregate);
        }

        private static ParameterCompareAggregateResult Aggregate(ParameterCompareAggregateResult result, CompareResult compareResult)
        {
            return Aggregate(result, Convert(compareResult));
        }

        private static ParameterCompareAggregateResult Convert(CompareResult compareResult)
        {
            switch (compareResult.Action)
            {
                case ParameterCompareResultAction.Equal:
                    return ParameterCompareAggregateResult.Equal;
                case ParameterCompareResultAction.NeedConvertToAsyncFunc:
                    return ParameterCompareAggregateResult.EqualOrCanBeConverting;
                case ParameterCompareResultAction.NotEqual:
                    return ParameterCompareAggregateResult.NotEqual;
                default:
                    throw new ArgumentOutOfRangeException(nameof(compareResult.Action), compareResult.Action, null);
            }
        }

        private static ParameterCompareAggregateResult Aggregate(ParameterCompareAggregateResult result, ParameterCompareAggregateResult newResult)
        {
            return ToInt(result) < ToInt(newResult) ? result : newResult;
        }

        private static int ToInt(ParameterCompareAggregateResult result)
        {
            switch (result)
            {
                case ParameterCompareAggregateResult.NotEqual:
                    return 10;
                case ParameterCompareAggregateResult.DifferentLength:
                    return 0;
                case ParameterCompareAggregateResult.Equal:
                    return 30;
                case ParameterCompareAggregateResult.EqualOrCanBeConverting:
                    return 50;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }
    }
}