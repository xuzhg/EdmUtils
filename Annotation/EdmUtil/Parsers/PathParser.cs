// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Annotation.EdmUtil.Commons;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    /// <summary>
    /// A parser to parse the requst Uri.
    /// for example: /users/{id | userPrincipalName}/contactFolders/{contactFolderId}/contacts 
    /// </summary>
    public static class PathParser
    {
        /// <summary>
        /// Parse the string like "/users/{id | userPrincipalName}/contactFolders/{contactFolderId}/contacts"
        /// to segments using the default settings.
        /// </summary>
        /// <param name="requestUri">the request uri string.</param>
        /// <param name="model">the IEdm model.</param>
        /// <returns>Null or <see cref="UriParser"/>.</returns>
        public static UriPath ParsePath(string requestUri, IEdmModel model)
        {
            return ParsePath(requestUri, model, PathParserSettings.Default);
        }

        /// <summary>
        /// Parse the string like "/users/{id | userPrincipalName}/contactFolders/{contactFolderId}/contacts"
        /// to segments
        /// </summary>
        /// <param name="requestUri">the request uri string.</param>
        /// <param name="model">the IEdm model.</param>
        /// <param name="settings">the setting.</param>
        /// <returns>Null or <see cref="UriParser"/>.</returns>
        public static UriPath ParsePath(string requestUri, IEdmModel model, PathParserSettings settings)
        {
            if (model == null || settings == null || String.IsNullOrEmpty(requestUri))
            {
                return null;
            }

            // TODO: process the one-drive uri escape function call

            string[] items = requestUri.Split('/');
            IList<PathSegment> segments = new List<PathSegment>();
            foreach (var item in items)
            {
                string trimedItem = item.Trim();
                if (string.IsNullOrEmpty(trimedItem))
                {
                    continue;
                }

                if (segments.Count == 0)
                {
                    CreateFirstSegment(trimedItem, model, segments, settings);
                }
                else
                {
                    CreateNextSegment(trimedItem, model, segments, settings);
                }
            }

            return new UriPath(segments);
        }

        /// <summary>
        /// Process the first segment in the request uri.
        /// The first segment could be only singleton/entityset/operationimport, doesn't consider the $metadata, $batch
        /// </summary>
        /// <param name="identifier">the whole identifier of this segment</param>
        /// <param name="model">The Edm model.</param>
        /// <param name="path">The out put of the path, because it may include the key segment.</param>
        /// <param name="settings">The Uri parser settings</param>
        internal static void CreateFirstSegment(string identifier, IEdmModel model, IList<PathSegment> path, PathParserSettings settings)
        {
            // the identifier maybe include the key, for example: ~/users({id})
            identifier = identifier.ExtractParenthesis(out string parenthesisExpressions);

            // Try to bind entity set or singleton
            if (TryBindNavigationSource(identifier, parenthesisExpressions, model, path, settings))
            {
                return;
            }

            // Try to bind operation import
            if (TryBindOperationImport(identifier, parenthesisExpressions, model, path, settings))
            {
                return;
            }

            throw new Exception($"Unknown kind of first segment: '{identifier}'");
        }

        /// <summary>
        /// Create the next segment for the request uri.
        /// </summary>
        /// <param name="identifier">The request segment uri literal</param>
        /// <param name="model">The Edm model.</param>
        /// <param name="path">The out of the path</param>
        /// <param name="settings">The parser settings</param>
        internal static void CreateNextSegment(string identifier, IEdmModel model, IList<PathSegment> path, PathParserSettings settings)
        {
            // GET /Users/{id}
            // GET /Users({id})
            // GET /me/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'{timezone_format}')

            // maybe key or function parameters
            identifier = identifier.ExtractParenthesis(out string parenthesisExpressions);

            // can be "property, navproperty"
            if (TryBindPropertySegment(identifier, parenthesisExpressions, model, path, settings))
            {
                return;
            }

            // bind to type cast.
            if (TryBindTypeCastSegment(identifier, parenthesisExpressions, model, path, settings))
            {
                return;
            }

            // bound operations
            if (TryBindOperations(identifier, parenthesisExpressions, model, path, settings))
            {
                return;
            }

            // Handle Key as Segment
            if (TryBindKeySegment("(" + identifier + ")", path))
            {
                return;
            }

            throw new Exception($"Unknown kind of segment: '{identifier}', previous segment: '{path.Last().Identifier}'.");
        }

        /// <summary>
        /// Try to bind the idenfier as navigation source segment,
        /// Append it into path.
        /// </summary>
        internal static bool TryBindNavigationSource(string identifier,
            string parenthesisExpressions, // the potention parenthesis expression after identifer
            IEdmModel model,
            IList<PathSegment> path, PathParserSettings settings)
        {
            IEdmNavigationSource source = model.ResolveNavigationSource(identifier, settings.EnableCaseInsensitive);
            IEdmEntitySet entitySet = source as IEdmEntitySet;
            IEdmSingleton singleton = source as IEdmSingleton;

            if (entitySet != null)
            {
                path.Add(new EntitySetSegment(entitySet, identifier));

                // can append parenthesis after entity set. it should be the key
                if (parenthesisExpressions != null)
                {
                    if (!TryBindKeySegment(parenthesisExpressions, path))
                    {
                        throw new Exception($"Unknown parenthesis '{parenthesisExpressions}' after an entity set '{identifier}'.");
                    }
                }

                return true;
            }
            else if (singleton != null)
            {
                path.Add(new SingletonSegment(singleton, identifier));

                // can't append parenthesis after singleton
                if (parenthesisExpressions != null)
                {
                    throw new Exception($"Unknown parenthesis '{parenthesisExpressions}' after a singleton '{identifier}'.");
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Try to bind the idenfier as operation import (function import or action import) segment,
        /// Append it into path.
        /// </summary>
        private static bool TryBindOperationImport(string identifier, string parenthesisExpressions,
            IEdmModel model, IList<PathSegment> path, PathParserSettings settings)
        {
            // split the parameter key/value pair
            parenthesisExpressions.ExtractKeyValuePairs(out IDictionary<string, string> parameters, out string remaining);
            IList<string> parameterNames = parameters == null ? null : parameters.Keys.ToList();

            IEdmOperationImport operationImport = OperationHelper.ResolveOperationImports(identifier, parameterNames, model, settings.EnableCaseInsensitive);
            if (operationImport != null)
            {
                operationImport.TryGetStaticEntitySet(model, out IEdmEntitySetBase entitySetBase);
                path.Add(new OperationImportSegment(operationImport, entitySetBase));
                if (remaining != null && operationImport.IsFunctionImport())
                {
                    IEdmFunction function = (IEdmFunction)operationImport.Operation;
                    if (function.IsComposable)
                    {
                        if (TryBindKeySegment(parenthesisExpressions, path))
                        {
                            return true;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Try to bind the idenfier as property segment,
        /// Append it into path.
        /// </summary>
        private static bool TryBindPropertySegment(string identifier, string parenthesisExpressions, IEdmModel model,
            IList<PathSegment> path,
            PathParserSettings settings)
        {
            PathSegment preSegment = path.LastOrDefault();
            if (preSegment == null || !preSegment.IsSingle)
            {
                return false;
            }

            IEdmStructuredType structuredType = preSegment.EdmType as IEdmStructuredType;
            if (structuredType == null)
            {
                return false;
            }

            IEdmProperty property = structuredType.ResolveProperty(identifier, settings.EnableCaseInsensitive);
            if (property == null)
            {
                return false;
            }

            PathSegment segment;
            if (property.PropertyKind == EdmPropertyKind.Navigation)
            {
                var navigationProperty = (IEdmNavigationProperty)property;

                IEdmNavigationSource navigationSource = null;
                if (preSegment.NavigationSource != null)
                {
                    IEdmPathExpression bindingPath;
                    navigationSource = preSegment.NavigationSource.FindNavigationTarget(navigationProperty, path, out bindingPath);
                }

                // Relationship between TargetMultiplicity and navigation property:
                //  1) EdmMultiplicity.Many <=> collection navigation property
                //  2) EdmMultiplicity.ZeroOrOne <=> nullable singleton navigation property
                //  3) EdmMultiplicity.One <=> non-nullable singleton navigation property
                segment = new NavigationSegment(navigationProperty, navigationSource, identifier);
            }
            else
            {
                segment = new PropertySegment((IEdmStructuralProperty)property, preSegment.NavigationSource, identifier);
            }

            path.Add(segment);

            if (parenthesisExpressions != null && !property.Type.IsCollection() && !property.Type.AsCollection().ElementType().IsEntity())
            {
                throw new Exception($"Invalid '{parenthesisExpressions}' after property '{identifier}'.");
            }

            TryBindKeySegment(parenthesisExpressions, path);
            return true;
        }

        /// <summary>
        /// Try to bind the parenthesisExpressions as key,
        /// parenthesisExpressions should have '(' and ')' wrapped.
        /// </summary>
        /// <param name="parenthesisExpressions">'(' and ')' wrapped string.</param>
        /// <param name="path">the decorated path.</param>
        internal static bool TryBindKeySegment(string parenthesisExpressions, IList<PathSegment> path)
        {
            // key segment cann't be the first segment.
            // key segment only apply to collection.
            PathSegment preSegment = path.LastOrDefault();
            if (preSegment == null || preSegment.IsSingle || string.IsNullOrEmpty(parenthesisExpressions))
            {
                return false;
            }

            IEdmEntityType targetEntityType;
            if (!preSegment.EdmType.IsEntityCollectionType(out targetEntityType))
            {
                // key segment only apply to collection of entity
                return false;
            }

            parenthesisExpressions.ExtractKeyValuePairs(out IDictionary<string, string> keys, out string remaining);
            var typeKeys = targetEntityType.Key().ToList();
            if (typeKeys.Count != keys.Count)
            {
                return false;
            }

            if (typeKeys.Count == 1)
            {
                if (keys.Keys.ElementAt(0) != String.Empty)
                {
                    if (typeKeys[0].Name != keys.Keys.ElementAt(0))
                    {
                        return false;
                    }
                }
            }
            else
            {
                foreach (var items in typeKeys)
                {
                    if (!keys.ContainsKey(items.Name))
                    {
                        return false;
                    }
                }
            }

            if (remaining != null)
            {
                // not allowed as (key=value)(....)
                throw new Exception($"Invalid key parathesis '{parenthesisExpressions}'.");
            }

            path.Add(new KeySegment(keys, targetEntityType, preSegment.NavigationSource));
            return true;
        }

        /// <summary>
        /// Try to bind namespace-qualified type cast segment.
        /// </summary>
        internal static bool TryBindTypeCastSegment(string identifier, string parenthesisExpressions, IEdmModel model,
            IList<PathSegment> path,
            PathParserSettings settings)
        {
            if (identifier == null || identifier.IndexOf('.') < 0)
            {
                // type cast should be namespace-qualified name
                return false;
            }

            PathSegment preSegment = path.LastOrDefault();
            if (preSegment == null)
            {
                // type cast should not be the first segment.
                return false;
            }

            IEdmSchemaType schemaType = model.ResolveType(identifier, settings.EnableCaseInsensitive);
            if (schemaType == null)
            {
                return false;
            }

            IEdmType targetEdmType = schemaType as IEdmType;
            if (targetEdmType == null)
            {
                return false;
            }

            IEdmType previousEdmType = preSegment.EdmType;
            bool isNullable = false;
            if (previousEdmType.TypeKind == EdmTypeKind.Collection)
            {
                previousEdmType = ((IEdmCollectionType)previousEdmType).ElementType.Definition;
                isNullable = ((IEdmCollectionType)previousEdmType).ElementType.IsNullable;
            }

            if (!targetEdmType.IsOrInheritsFrom(previousEdmType) && !previousEdmType.IsOrInheritsFrom(targetEdmType))
            {
                throw new Exception($"Type cast {targetEdmType.FullTypeName()} has no relationship with previous {previousEdmType.FullTypeName()}.");
            }

            // We want the type of the type segment to be a collection if the previous segment was a collection
            IEdmType actualTypeOfTheTypeSegment = targetEdmType;
            if (preSegment.EdmType.TypeKind == EdmTypeKind.Collection)
            {
                var actualEntityTypeOfTheTypeSegment = targetEdmType as IEdmEntityType;
                if (actualEntityTypeOfTheTypeSegment != null)
                {
                    actualTypeOfTheTypeSegment = new EdmCollectionType(new EdmEntityTypeReference(actualEntityTypeOfTheTypeSegment, isNullable));
                }
                else
                {
                    // Complex collection supports type cast too.
                    var actualComplexTypeOfTheTypeSegment = actualTypeOfTheTypeSegment as IEdmComplexType;
                    if (actualComplexTypeOfTheTypeSegment != null)
                    {
                        actualTypeOfTheTypeSegment = new EdmCollectionType(new EdmComplexTypeReference(actualComplexTypeOfTheTypeSegment, isNullable));
                    }
                    else
                    {
                        throw new Exception($"Invalid type cast of {identifier}, it should be entity or complex.");
                    }
                }
            }

            TypeSegment typeCast = new TypeSegment(actualTypeOfTheTypeSegment, preSegment.EdmType, preSegment.NavigationSource, identifier);
            path.Add(typeCast);

            TryBindKeySegment(parenthesisExpressions, path);

            return true;
        }

        /// <summary>
        /// Try to bind the idenfier as bound operation segment,
        /// Append it into path.
        /// </summary>
        internal static bool TryBindOperations(string identifier, string parenthesisExpressions,
            IEdmModel model, IList<PathSegment> path, PathParserSettings settings)
        {
            PathSegment preSegment = path.LastOrDefault();
            if (preSegment == null)
            {
                // bound operation cannot be the first segment.
                return false;
            }

            IEdmType bindingType = preSegment.EdmType;

            // operation
            parenthesisExpressions.ExtractKeyValuePairs(out IDictionary<string, string> parameters, out string remaining);
            IList<string> parameterNames = parameters == null ? null : parameters.Keys.ToList();

            IEdmOperation operation = OperationHelper.ResolveOperations(identifier, parameterNames, bindingType, model, settings.EnableCaseInsensitive);
            if (operation != null)
            {
                IEdmEntitySetBase targetset = null;
                if (operation.ReturnType != null)
                {
                    IEdmNavigationSource source = preSegment == null ? null : preSegment.NavigationSource;
                    targetset = operation.GetTargetEntitySet(source, model);
                }

                path.Add(new OperationSegment(operation, targetset));

                if (remaining != null && operation.IsFunction())
                {
                    IEdmFunction function = (IEdmFunction)operation;
                    if (function.IsComposable)
                    {
                        // to process the ~/ .../ NS.Function(p1 ={ abc})({ id})
                        if (TryBindKeySegment(parenthesisExpressions, path))
                        {
                            return true;
                        }
                    }
                }

                return true;
            }

            return false;
        }
    }
}
