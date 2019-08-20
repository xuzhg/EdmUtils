
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil.Commons
{
    public static class EdmExtensions
    {
        internal static bool IsStructuredCollectionType(this IEdmTypeReference typeReference)
        {
            return typeReference.Definition.IsStructuredCollectionType();
        }

        internal static bool IsStructuredCollectionType(this IEdmType type)
        {
            IEdmCollectionType collectionType = type as IEdmCollectionType;

            if (collectionType == null
                || (collectionType.ElementType != null
                    && (collectionType.ElementType.TypeKind() != EdmTypeKind.Entity && collectionType.ElementType.TypeKind() != EdmTypeKind.Complex)))
            {
                return false;
            }

            return true;
        }

        public static bool IsEntityOrEntityCollectionType(this IEdmType edmType, out IEdmEntityType entityType)
        {
            if (edmType.TypeKind == EdmTypeKind.Entity)
            {
                entityType = (IEdmEntityType)edmType;
                return true;
            }

            if (edmType.TypeKind != EdmTypeKind.Collection)
            {
                entityType = null;
                return false;
            }

            entityType = ((IEdmCollectionType)edmType).ElementType.Definition as IEdmEntityType;
            return entityType != null;
        }
    }
}
