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
    public class PathParser
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OperationImportSegment"/> class.
        /// </summary>
        /// <param name="model">The Edm model used in parsing.</param>
        public PathParser(IEdmModel model)
            : this(model, new PathParserSettings())
        { }

        public PathParser(IEdmModel model, PathParserSettings settings)
        {
            EdmModel = model ?? throw new ArgumentNullException(nameof(model));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Gets the Edm model.
        /// </summary>
        public IEdmModel EdmModel { get; }

        /// <summary>
        /// Gets the Uri parser settings.
        /// </summary>
        public PathParserSettings Settings { get; }

#if false
        /// <summary>
        /// Parse the string like "/users/{id | userPrincipalName}/contactFolders/{contactFolderId}/contacts"
        /// to segments
        /// </summary>
        public virtual IList<PathSegment> Parse(string requestUri)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                return null;
            }

            string[] items = requestUri.Split('/');
            IList<PathSegment> segments = new List<PathSegment>();
            foreach (var item in items)
            {
                string trimedItem = item.Trim();
                if (string.IsNullOrEmpty(trimedItem))
                {
                    // skip like "~////abc/xyz//
                    continue;
                }

                if (segments.Count == 0)
                {
                    CreateFirstSegment(trimedItem, model, segments);
                }
                else
                {
                    CreateNextSegment(trimedItem, model, segments);
                }
            }

            return segments;
        }
#endif

        /// <summary>
        /// Parse the string like "/users/{id | userPrincipalName}/contactFolders/{contactFolderId}/contacts"
        /// to segments
        /// </summary>
        public static IList<PathSegment> Parse(string requestUri, IEdmModel model)
        {
            if (String.IsNullOrEmpty(requestUri))
            {
                return null;
            }

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
                    CreateFirstSegment(trimedItem, model, segments);
                }
                else
                {
                    CreateNextSegment(trimedItem, model, segments);
                }
            }

            return segments;
        }

        internal static void CreateFirstSegment(string identifier, // the whole identifier of this segment
            IEdmModel model,
            IList<PathSegment> path,
            bool enableCaseInsensitive = false)
        {
            // We only process the singleton/entityset/operationimport
            // the identifier maybe include the key, for example: ~/users({id})
            identifier = identifier.ExtractParenthesis(out string parenthesisExpressions);

            // Try to bind entity set or singleton
            if (TryBindNavigationSource(identifier, parenthesisExpressions, model, path, enableCaseInsensitive))
            {
                return;
            }

            // Try to bind operation import
            if (TryBindOperationImport(identifier, parenthesisExpressions, model, path, enableCaseInsensitive))
            {
                return;
            }

            throw new Exception($"Unknow kind of first segment: '{identifier}'");
        }

        internal static void CreateNextSegment(string identifier, IEdmModel model, IList<PathSegment> path,
            bool enableCaseInsensitive = false)
        {
            PathSegment preSegment = path.Last();

            // GET /Users/{id}
            // GET /Users({id})
            // GET /me/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'{timezone_format}')

            // maybe key or function parameters
            identifier = identifier.ExtractParenthesis(out string parenthesisExpressions);

            // can be "property, navproperty"
            if (TryBindPropertySegment(identifier, parenthesisExpressions, model, path, enableCaseInsensitive))
            {
                return;
            }

            // type cast, 
            if (identifier.IndexOf('.') >= 0)
            {
                if (TryBindTypeCast(preSegment, identifier, model, parenthesisExpressions, path))
                {
                    return;
                }
            }

            // bound operations
            if (TryBindOperations(preSegment, identifier, parenthesisExpressions, model, path))
            {
                return;
            }

            // Handle Key Segment
            if (TryBindKeySegment("(" + identifier + ")", path))
            {
                return;
            }

            throw new Exception($"Unknow kind of segment: {identifier}");
        }

        /// <summary>
        /// Try to bind the idenfier as navigation source segment,
        /// Append it into path.
        /// </summary>
        internal static bool TryBindNavigationSource(string identifier,
            string parenthesisExpressions, // the potention parenthesis expression after identifer
            IEdmModel model,
            IList<PathSegment> path, bool enableCaseInsensitive)
        {
            IEdmNavigationSource source = model.ResolveNavigationSource(identifier, enableCaseInsensitive);
            IEdmEntitySet entitySet = source as IEdmEntitySet;
            IEdmSingleton singleton = source as IEdmSingleton;

            if (entitySet != null)
            {
                path.Add(new EntitySetSegment(entitySet));

                // can append parenthesis after entity set. it should be the key
                if (parenthesisExpressions != null)
                {
                    if (!TryBindKeySegment(parenthesisExpressions, path))
                    {
                        throw new Exception($"Unknown parentheis '{parenthesisExpressions}' after an entity set '{identifier}'.");
                    }
                }

                return true;
            }
            else if (singleton != null)
            {
                path.Add(new SingletonSegment(singleton));

                // can't append parenthesis after singleton
                if (parenthesisExpressions != null)
                {
                    throw new Exception($"Unknown parentheis '{parenthesisExpressions}' after a singleton '{identifier}'.");
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
            IEdmModel model, IList<PathSegment> path, bool enableCaseInsensitive = false)
        {
            // split the parameter key/value pair
            parenthesisExpressions.ExtractKeyValuePairs(out IDictionary<string, string> parameters, out string remaining);
            IList<string> parameterNames = parameters == null ? null : parameters.Keys.ToList();

            IEdmOperationImport operationImport = OperationHelper.ResolveOperationImports(identifier, parameterNames, model);
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

        private static bool TryBindPropertySegment(string identifier, string parenthesisExpressions, IEdmModel model, IList<PathSegment> path,
            bool enableCaseInsensitive = false)
        {
            PathSegment preSegment = path.Last();
            if (!preSegment.IsSingle)
            {
                return false;
            }
            IEdmStructuredType structuredType = preSegment.EdmType as IEdmStructuredType;
            if (structuredType == null)
            {
                IEdmCollectionType collectionType = preSegment.EdmType as IEdmCollectionType;
                if (collectionType != null)
                {
                    structuredType = collectionType.ElementType.Definition as IEdmStructuredType;
                }
            }

            if (structuredType == null)
            {
                return false;
            }

            IEdmProperty property = structuredType.ResolveProperty(identifier, enableCaseInsensitive);
            if (property == null)
            {
                return false;
            }

            PathSegment segment = null;
            if (property.PropertyKind == EdmPropertyKind.Navigation)
            {
                var navigationProperty = (IEdmNavigationProperty)property;

                IEdmNavigationSource navigationSource = null;
                if (preSegment.NavigationSource != null)
                {
                    //  IEdmPathExpression bindingPath;
                    //  navigationSource = previous.NavigationSource.FindNavigationTarget(navigationProperty, BindingPathHelper.MatchBindingPath, this.parsedSegments, out bindingPath);
                }

                // Relationship between TargetMultiplicity and navigation property:
                //  1) EdmMultiplicity.Many <=> collection navigation property
                //  2) EdmMultiplicity.ZeroOrOne <=> nullable singleton navigation property
                //  3) EdmMultiplicity.One <=> non-nullable singleton navigation property
                //
                // According to OData Spec CSDL 7.1.3:
                //  1) non-nullable singleton navigation property => navigation source required
                //  2) the other cases => navigation source optional
                if (navigationProperty.TargetMultiplicity() == EdmMultiplicity.One
                    && navigationSource is IEdmUnknownEntitySet)
                {
                    // Specifically not throwing ODataUriParserException since it's more an an internal server error
                    throw new Exception("TODO: ");
                }

                segment = new NavigationSegment(navigationProperty, navigationSource);
            }
            else
            {
                segment = new PropertySegment((IEdmStructuralProperty)property, preSegment.NavigationSource);
            }

            path.Add(segment);

            if (parenthesisExpressions != null && !property.Type.IsCollection() && !property.Type.AsCollection().ElementType().IsEntity())
            {
                throw new Exception("TODO: ");
            }

            TryBindKeySegment(parenthesisExpressions, path);

            return true;
        }

        private static bool TryBindKeySegment(string parenthesisExpressions, IList<PathSegment> path)
        {
            if (parenthesisExpressions == null || path.Count == 0)
            {
                return false;
            }

            PathSegment preSegment = path.Last();
            if (preSegment.IsSingle)
            {
                return false;
            }

            IEdmEntityType targetEntityType;
            if (preSegment.EdmType == null || !preSegment.EdmType.IsEntityOrEntityCollectionType(out targetEntityType))
            {
                return false;
            }

            parenthesisExpressions.ExtractKeyValuePairs(out IDictionary<string, string> keys, out string remaining);
            if (remaining != null)
            {
                throw new Exception($"Invalid key parathesis {parenthesisExpressions}.");
            }

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

            path.Add(new KeySegment(keys, targetEntityType, preSegment.NavigationSource));
            return true;
        }

        internal static bool TryBindTypeCast(PathSegment preSegment, string identifier, IEdmModel model, string parenthesisExpressions, IList<PathSegment> path)
        {
            IEdmSchemaType schemaType = model.FindType(identifier);
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
            if (previousEdmType.TypeKind == EdmTypeKind.Collection)
            {
                previousEdmType = ((IEdmCollectionType)previousEdmType).ElementType.Definition;
            }

            if (!targetEdmType.IsOrInheritsFrom(previousEdmType) && !previousEdmType.IsOrInheritsFrom(targetEdmType))
            {
                throw new Exception($"type cast {targetEdmType.FullTypeName()} has no relationship with previous {previousEdmType.FullTypeName()}.");
            }

            // // We want the type of the type segment to be a collection if the previous segment was a collection
            IEdmType actualTypeOfTheTypeSegment = targetEdmType;
            if (preSegment.EdmType.TypeKind == EdmTypeKind.Collection)
            {
                var actualEntityTypeOfTheTypeSegment = targetEdmType as IEdmEntityType;
                if (actualEntityTypeOfTheTypeSegment != null)
                {
                    actualTypeOfTheTypeSegment = new EdmCollectionType(new EdmEntityTypeReference(actualEntityTypeOfTheTypeSegment, false));
                }
                else
                {
                    // Complex collection supports type cast too.
                    var actualComplexTypeOfTheTypeSegment = actualTypeOfTheTypeSegment as IEdmComplexType;
                    if (actualComplexTypeOfTheTypeSegment != null)
                    {
                        actualTypeOfTheTypeSegment = new EdmCollectionType(new EdmComplexTypeReference(actualComplexTypeOfTheTypeSegment, false));
                    }
                    else
                    {
                        throw new Exception($"Invlid type cast of {identifier}, it should be entity or complex");
                    }
                }
            }

            TypeSegment typeCast = new TypeSegment(actualTypeOfTheTypeSegment, previousEdmType, preSegment.NavigationSource);
            path.Add(typeCast);

            TryBindKeySegment(parenthesisExpressions, path);

            return true;
        }

        internal static bool TryBindOperations(PathSegment preSegment, string identifier, string parenthesisExpressions, 
            IEdmModel model, IList<PathSegment> path)
        {
            IEdmType bindingType = preSegment.EdmType;

            // operation
            parenthesisExpressions.ExtractKeyValuePairs(out IDictionary<string, string> parameters, out string remaining);
            IList<string> parameterNames = parameters == null ? null : parameters.Keys.ToList();

            IEdmOperation operation = OperationHelper.ResolveOperations(identifier, parameterNames, bindingType, model);
            if (operation != null)
            {
                path.Add(new OperationSegment(operation));

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
