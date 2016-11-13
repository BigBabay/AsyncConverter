using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Util;

namespace AsyncConverter
{
    public static class TypeHelper
    {
        public static bool IsGenericTaskOf([CanBeNull] this IType returnType, [CanBeNull] IType originalReturnType)
        {
            if (returnType == null || originalReturnType == null)
                return false;
            if (!returnType.IsGenericTask())
                return false;

            var resurnDeclaredType = returnType as IDeclaredType;
            if (resurnDeclaredType == null)
                return false;

            var substitution = resurnDeclaredType.GetSubstitution();
            if (substitution.IsEmpty())
                return false;

            var innerCandidateReturnMethod = substitution.Apply(substitution.Domain[0]);
            return innerCandidateReturnMethod.Equals(originalReturnType);
        }
        [Pure]
        [ContractAnnotation("null => false")]
        public static bool IsFunc([CanBeNull]this IType type)
        {
            var declaredType = type as IDeclaredType;
            if (declaredType == null)
                return false;

            var clrTypeName = declaredType.GetClrName();
            if (!clrTypeName.Equals(PredefinedType.FUNC_FQN))
                return clrTypeName.FullName.StartsWith(PredefinedType.FUNC_FQN + "`");
            return true;
        }
    }
}