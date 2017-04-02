using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Util;
using IType = JetBrains.ReSharper.Psi.IType;
using ITypeParameter = JetBrains.ReSharper.Psi.ITypeParameter;

namespace AsyncConverter.Helpers
{
    public static class TypeHelper
    {
        [Pure]
        [ContractAnnotation("type:null => false; otherType:null => false")]
        public static bool IsTaskOf([CanBeNull] this IType type, [CanBeNull] IType otherType)
        {
            if (type.IsTask() && otherType.IsVoid())
                return true;
            return type.IsGenericTaskOf(otherType);
        }

        [Pure]
        [ContractAnnotation("type:null => false; otherType:null => false")]
        public static bool IsGenericTaskOf([CanBeNull] this IType type, [CanBeNull] IType otherType)
        {
            if (type == null || otherType == null)
                return false;
            if (!type.IsGenericTask())
                return false;

            var taskDeclaredType = type as IDeclaredType;
            if (taskDeclaredType == null)
                return false;

            var substitution = taskDeclaredType.GetSubstitution();
            if (substitution.IsEmpty())
                return false;
            var meaningType = substitution.Apply(substitution.Domain[0]);
            return meaningType.IsEquals(otherType);
        }

        [Pure]
        [ContractAnnotation("null => false")]
        public static bool IsFunc([CanBeNull]this IType type)
        {
            var declaredType = type as IDeclaredType;
            if (declaredType == null)
                return false;

            var clrTypeName = declaredType.GetClrName();
            return clrTypeName.Equals(PredefinedType.FUNC_FQN) || clrTypeName.FullName.StartsWith(PredefinedType.FUNC_FQN + "`");
        }

        [Pure]
        [ContractAnnotation("null => false")]
        public static bool IsGenericIQueryable([CanBeNull]this IType type)
        {
            return TypesUtil.IsPredefinedTypeFromAssembly(type, PredefinedType.GENERIC_IQUERYABLE_FQN, assembly => assembly.IsMscorlib);
        }

        [Pure]
        [ContractAnnotation("null => false")]
        public static bool IsEnumerableClass([CanBeNull]this ITypeElement type)
        {
            if (type == null)
                return false;
            var typeName = type.GetClrName();
            return typeName.Equals(PredefinedType.ENUMERABLE_CLASS);
        }

        public static bool IsEquals([NotNull]this IType type, [NotNull] IType otherType)
        {
            if (!type.IsOpenType && !otherType.IsOpenType)
                return Equals(type, otherType);
            if (!IsEqualTypeGroup(type, otherType))
                return false;
            var scalarType = type.GetScalarType();
            var otherScalarType = otherType.GetScalarType();
            if (scalarType == null || otherScalarType == null)
                return false;
            if (scalarType.Classify != otherScalarType.Classify)
                return false;
            var typeElement1 = scalarType.GetTypeElement();
            var typeElement2 = otherScalarType.GetTypeElement();
            if (typeElement1 == null || typeElement2 == null)
                return false;
            var typeParameter1 = typeElement1 as ITypeParameter;
            var typeParameter2 = typeElement2 as ITypeParameter;
            if (typeParameter1 != null && typeParameter2 != null)
            {
                if(typeParameter1.HasDefaultConstructor != typeParameter2.HasDefaultConstructor
                    || typeParameter1.TypeConstraints.Count != typeParameter2.TypeConstraints.Count)
                    return false;
                if (typeParameter1.TypeConstraints.Where((t, i) => !t.IsEquals(typeParameter2.TypeConstraints[i])).Any())
                {
                    return false;
                }
            }
            return EqualSubstitutions(typeElement1, scalarType.GetSubstitution(), typeElement2, otherScalarType.GetSubstitution());
        }

        public static bool IsAsyncDelegate([NotNull] this IType type, [NotNull] IType otherType)
        {
            if (type.IsAction() && otherType.IsFunc())
            {
                var parameterDeclaredType = otherType as IDeclaredType;
                var substitution = parameterDeclaredType?.GetSubstitution();
                if (substitution?.Domain.Count != 1)
                    return false;

                var valuableType = substitution.Apply(substitution.Domain[0]);
                return valuableType.IsTask();
            }
            if (type.IsFunc() && otherType.IsFunc())
            {
                var parameterDeclaredType = otherType as IDeclaredType;
                var originalParameterDeclaredType = type as IDeclaredType;
                var substitution = parameterDeclaredType?.GetSubstitution();
                var originalSubstitution = originalParameterDeclaredType?.GetSubstitution();
                if (substitution == null || substitution.Domain.Count != originalSubstitution?.Domain.Count)
                    return false;

                var i = 0;
                for (; i < substitution.Domain.Count - 1; i++)
                {
                    var genericType = substitution.Apply(substitution.Domain[i]);
                    var originalGenericType = originalSubstitution.Apply(originalSubstitution.Domain[i]);
                    if (!genericType.Equals(originalGenericType))
                        return false;
                }
                var returnType = substitution.Apply(substitution.Domain[i]);
                var originalReturnType = originalSubstitution.Apply(originalSubstitution.Domain[i]);
                return returnType.IsGenericTaskOf(originalReturnType);
            }
            return false;
        }

        private static bool IsEqualTypeGroup([NotNull] IType sourceType, [NotNull] IType targetType)
        {
            if (sourceType.IsOpenType != targetType.IsOpenType)
                return false;
            if (sourceType is IDeclaredType && targetType is IDeclaredType || sourceType is IArrayType && targetType is IArrayType)
                return true;
            if (sourceType is IPointerType && targetType is IPointerType)
                return true;
            return false;
        }

        private static bool EqualSubstitutions([NotNull] ITypeElement referenceOwner, [NotNull] ISubstitution referenceSubstitution, [NotNull] ITypeElement originOwner, [NotNull] ISubstitution originSubstitution)
        {
            foreach (var substitution1 in referenceOwner.GetAncestorSubstitution(originOwner))
            {
                var substitution2 = substitution1.Apply(referenceSubstitution);
                foreach (var typeParameter in substitution2.Domain)
                {
                    if (originSubstitution.HasInDomain(typeParameter) && !substitution2[typeParameter].IsEquals(originSubstitution[typeParameter]))
                        return false;
                }
            }
            return true;
        }
    }
}