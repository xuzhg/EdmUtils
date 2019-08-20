
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Annotation.EdmUtil.Commons
{
    public static class EdmExtensions
    {
        public static IEdmProperty ResolveProperty(this IEdmStructuredType type, string propertyName, bool enableCaseInsensitive = false)
        {
            IEdmProperty property = type.FindProperty(propertyName);
            if (property != null || !enableCaseInsensitive)
            {
                return property;
            }

            var result = type.Properties()
            .Where(_ => string.Equals(propertyName, _.Name, StringComparison.OrdinalIgnoreCase))
            .ToList();

            return result.SingleOrDefault();
        }

        public static IEdmNavigationSource ResolveNavigationSource(this IEdmModel model,
            string identifier,
            bool enableCaseInsensitive = false)
        {
            IEdmNavigationSource navSource = model.FindDeclaredNavigationSource(identifier);
            if (navSource != null || !enableCaseInsensitive)
            {
                return navSource;
            }

            IEdmEntityContainer container = model.EntityContainer;
            if (container == null)
            {
                return null;
            }

            var result = container.Elements.OfType<IEdmNavigationSource>()
                .Where(source => string.Equals(identifier, source.Name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (result.Count > 1)
            {
                throw new Exception($"More than one navigation sources match the name '{identifier}' were found in model.");
            }

            return result.SingleOrDefault();
        }

        public static IEnumerable<IEdmOperationImport> ResolveOperationImports(this IEdmModel model,
            string identifier,
            bool enableCaseInsensitive = false)
        {
            IEnumerable<IEdmOperationImport> results = model.FindDeclaredOperationImports(identifier);
            if (results.Any() || !enableCaseInsensitive)
            {
                return results;
            }

            IEdmEntityContainer container = model.EntityContainer;
            if (container == null)
            {
                return null;
            }

            return container.Elements.OfType<IEdmOperationImport>()
                .Where(source => string.Equals(identifier, source.Name, StringComparison.OrdinalIgnoreCase));
        }

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
