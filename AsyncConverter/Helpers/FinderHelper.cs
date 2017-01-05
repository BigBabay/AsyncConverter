using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Search;

namespace AsyncConverter.Helpers
{
    public static class FinderHelper
    {
        [NotNull]
        [ItemNotNull]
        public static IList<TOverridableMember> FindImplementingMembers<TOverridableMember>([NotNull] this TOverridableMember overridableMember, [CanBeNull] IProgressIndicator pi = null)
            where TOverridableMember : class, IOverridableMember
        {
            var found = new List<TOverridableMember>();
            overridableMember
                .GetPsiServices()
                .Finder
                .FindImplementingMembers(overridableMember, overridableMember.GetSearchDomain(),
                    new FindResultConsumer(findResult =>
                                           {
                                               var resultOverridableMember = findResult as FindResultOverridableMember;
                                               var result = resultOverridableMember?.OverridableMember as TOverridableMember;
                                               if (result != null)
                                                   found.Add(result);
                                               return FindExecution.Continue;
                                           }), true, pi ?? NullProgressIndicator.Instance);
            return found;
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<IMethod> FindBaseMethods([NotNull] this IMethod method, [CanBeNull] IProgressIndicator pi = null)
        {
            var finder = method
                .GetPsiServices()
                .Finder;
            return InnerFindBaseMethods(finder, method, pi ?? NullProgressIndicator.Instance);
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<IMethod> InnerFindBaseMethods([NotNull] IFinder finder, [NotNull] IMethod method, [NotNull] IProgressIndicator pi)
        {
            return finder
                .FindImmediateBaseElements(method, pi)
                .OfType<IMethod>()
                .SelectMany(innerMethod => innerMethod.FindBaseMethods())
                .Concat(new[] {method});
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<IMethod> FindAllHierarchy([NotNull] this IMethod method, [CanBeNull] IProgressIndicator pi = null)
        {
            var finder = method
                .GetPsiServices()
                .Finder;
            return InnerFindAllHierarchy(finder, method, pi ?? NullProgressIndicator.Instance);
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<IMethod> InnerFindAllHierarchy([NotNull] IFinder finder, [NotNull] IMethod method, [NotNull] IProgressIndicator pi)
        {
            var immediateBaseMethods = finder
                    .FindImmediateBaseElements(method, pi)
                    .OfType<IMethod>()
                    .ToArray();
            return immediateBaseMethods.Any()
                ? immediateBaseMethods.SelectMany(innerMethod => innerMethod.FindAllHierarchy())
                : new [] { method }.Concat(method.FindImplementingMembers());
        }
    }
}