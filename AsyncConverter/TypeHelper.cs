using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;

namespace AsyncConverter
{
    public static class TypeHelper
    {
        [Pure]
        public static bool IsGenericTaskOf([CanBeNull] this IType taskType, [CanBeNull] IType otherType)
        {
            if (taskType == null || otherType == null)
                return false;
            if (!taskType.IsGenericTask())
                return false;

            var taskDeclaredType = taskType as IDeclaredType;
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

        public static bool IsEquals([NotNull]this IType type, [NotNull] IType otherType)
        {
            if (!type.IsOpenType && !otherType.IsOpenType)
                return Equals(type, otherType);
            if (!IsEqualTypeGroup(type, otherType))
                return false;
            var scalarType1 = type.GetScalarType();
            var otherScalarType = otherType.GetScalarType();
            if (scalarType1 == null || otherScalarType == null)
                return false;
            var typeElement1 = scalarType1.GetTypeElement();
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
                if (typeParameter1.TypeConstraints.Any(t => !t.IsEquals(t)))
                {
                    return false;
                }
            }
            return Equals(typeElement1, typeElement2) && EqualSubstitutions(typeElement1, scalarType1.GetSubstitution(), typeElement2, otherScalarType.GetSubstitution());
        }

        /// <summary>
        /// Return value indicating the specific type occurs in this open declared type.
        /// If this type contains value type (for example for List`T &amp; T it returns true)
        /// </summary>
        private static bool ContainsOpenType(IDeclaredType thisType, IDeclaredType value)
        {
            bool isAny = false;
            value.Accept(new DelegatingTypeVisitor
                         {
                             ProcessDeclaredType = d =>
                                                   {
                                                       if (!d.IsOpenType || !Equals(d, thisType))
                                                           return;
                                                       isAny = true;
                                                   }
                         });
            return isAny;
        }

        private static bool IsEqualTypeGroup([NotNull] IType sourceType, [NotNull] IType targetType)
        {
            if (sourceType is IDeclaredType && targetType is IDeclaredType || sourceType is IArrayType && targetType is IArrayType)
                return true;
            if (sourceType is IPointerType)
                return targetType is IPointerType;
            return false;
        }

        private static bool EqualSubstitutions([NotNull] IMethod referenceOwner, [NotNull] ISubstitution referenceSubstitution, [NotNull] IMethod originOwner, [NotNull] ISubstitution originSubstitution)
        {
            ITypeElement containingType1 = referenceOwner.GetContainingType();
            ITypeElement containingType2 = originOwner.GetContainingType();
            if (containingType1 == null || containingType2 == null || (!EqualSubstitutions(containingType1, referenceSubstitution, containingType2, originSubstitution) || referenceOwner.TypeParameters.Count != originOwner.TypeParameters.Count))
                return false;
            for (int index = 0; index < referenceOwner.TypeParameters.Count; ++index)
            {
                if (!IsEquals(referenceSubstitution[referenceOwner.TypeParameters[index]], originSubstitution[originOwner.TypeParameters[index]]))
                    return false;
            }
            return true;
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