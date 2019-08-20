
using System;
using System.Collections.Generic;
using System.Linq;
using Annotation.EdmUtil.Commons;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    public class UriParser
    {
        private IEdmModel _model;

        public UriParser(IEdmModel model)
        {
            _model = model;
        }

        /// <summary>
        /// Parse the string like "/users/{id | userPrincipalName}/contactFolders/{contactFolderId}/contacts"
        /// to segments
        /// </summary>
        public static IList<PathSegment> Parse(string requestUri, IEdmModel model)
        {
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

        internal static void CreateFirstSegment(string identifier, IEdmModel model, IList<PathSegment> path)
        {
            // We only process the singleton/entityset/operationimport
            // the identifier maybe include the key, for example: ~/users({id})
            identifier = identifier.ExtractParenthesis(out string parenthesisExpressions);

            IEdmEntitySet entitySet = model.FindDeclaredEntitySet(identifier);
            if (entitySet != null)
            {
                path.Add(new EntitySetSegment(entitySet));

                if (parenthesisExpressions != null)
                {
                    if (TryBindKeySegment(parenthesisExpressions, path))
                    {
                        return;
                    }

                    throw new Exception($"Unknown the parentheis {parenthesisExpressions} after an entity set {identifier}.");
                }

                return;
            }

            IEdmSingleton singleton = model.FindDeclaredSingleton(identifier);
            if (singleton != null)
            {
                path.Add(new SingletonSegment(singleton));
                if (parenthesisExpressions != null)
                {
                    throw new Exception($"Unknown the parentheis {parenthesisExpressions} after a singleton {identifier}.");
                }

                return;
            }

            // operation import
            parenthesisExpressions.ExtractKeyValuePairs(out IDictionary<string, string> parameters, out string remaining);
            IList<string> parameterNames = parameters == null ? null : parameters.Keys.ToList();

            IEdmOperationImport import = OperationHelper.ResolveOperationImports(identifier, parameterNames, model);
            if (import != null)
            {
                path.Add(new OperationImportSegment(import));

                if (remaining != null && import.IsFunctionImport())
                {
                    IEdmFunction function = (IEdmFunction)import.Operation;
                    if (function.IsComposable)
                    {
                        if (TryBindKeySegment(parenthesisExpressions, path))
                        {
                            return;
                        }
                    }
                }

                return;
            }

            throw new Exception($"Unknow type of first segment: {identifier}");
        }

        internal static void CreateNextSegment(string identifier, IEdmModel model, IList<PathSegment> path)
        {
            PathSegment preSegment = path.Last();

            // GET /Users/{id}
            // GET /Users({id})
            // GET /me/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'{timezone_format}')

            // maybe key or function parameters
            identifier = identifier.ExtractParenthesis(out string parenthesisExpressions);

            if (preSegment.IsSingle)
            {
                // can be "property, navproperty"
                IEdmProperty property;
                if (TryBindProperty(preSegment, identifier, out property))
                {
                    CreatePropertySegment(preSegment, property, parenthesisExpressions, path);
                    return;
                }
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
            if (TryBindOperations(preSegment, identifier, parenthesisExpressions))
            {
                return;
            }

            // Handle Key Segment
            if (TryBindKeySegment("(" + identifier + ")", path))
            {
                return;
            }

            throw new Exception($"Unknow type of first segment: {identifier}");
        }

        private static void CreatePropertySegment(PathSegment previous, IEdmProperty property, string parenthesisExpressions, IList<PathSegment> path)
        {
            PathSegment segment = null;

            if (property.PropertyKind == EdmPropertyKind.Navigation)
            {
                var navigationProperty = (IEdmNavigationProperty)property;

                IEdmNavigationSource navigationSource = null;
                if (previous.NavigationSource != null)
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

                path.Add(new NavigationSegment(navigationProperty, navigationSource));
            }
            else
            {
                segment = new PropertySegment((IEdmStructuralProperty)property, previous.NavigationSource);
            }

            path.Add(segment);

            if (parenthesisExpressions != null && !property.Type.IsCollection() && !property.Type.AsCollection().ElementType().IsEntity())
            {
                throw new Exception("TODO: ");
            }

            TryBindKeySegment(parenthesisExpressions, path);
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

        private static bool TryBindOperations(PathSegment preSegment, string identifier, string parenthesisExpressions)
        {
            IEdmType bindingType = preSegment.EdmType;

            // operation
            parenthesisExpressions.ExtractKeyValuePairs(out IDictionary<string, string> parameters, out string remaining);
            IList<string> parameterNames = parameters == null ? null : parameters.Keys.ToList();

            IEdmOperation operation = OperationHelper.ResolveOperations(identifier, parameterNames, bindingType, model);
            if (import != null)
            {
                path.Add(new OperationSegment(import));

                if (remaining != null && import.IsFunctionImport())
                {
                    IEdmFunction function = (IEdmFunction)import.Operation;
                    if (function.IsComposable)
                    {
                        if (TryBindKeySegment(parenthesisExpressions, path))
                        {
                            return;
                        }
                    }
                }

                return;
            }

            // TODO: do we need to process the ~/.../NS.Function(p1={abc})({id})
            // TODO: do we need to process the ~/.../NS.Function(p1={abc})/{id}
            return false;
        }

        private PathSegment CreatePropertySegment(PathSegment previous, IEdmProperty property, string parenthesisExpressions)
        {
            PathSegment segment;
            if (property.PropertyKind == EdmPropertyKind.Navigation)
            {
                IEdmNavigationProperty navigationProperty = (IEdmNavigationProperty)property;

                // Calculate the navigation source binding for this navigation property
                // Containment or non-containment

                segment = new NavigationSegment(navigationProperty);
            }
            else
            {
                segment = new PropertySegment((IEdmStructuralProperty)property);
            }

            return segment;
        }

        private static bool TryBindProperty(PathSegment preSegment, string identifier, out IEdmProperty property)
        {
            property = null;
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

            property = structuredType.FindProperty(identifier);
            return property != null;
        }

        

        private static string ExtractParenthesis(string identifier, out IDictionary<string, string> parenthesisExpressions)
        {
            parenthesisExpressions = null;

            int parenthesisStart = identifier.IndexOf('(');
            if (parenthesisStart >= 0)
            {
                if (identifier[identifier.Length - 1] != ')')
                {
                    throw new Exception($"Invalid identifier {identifier}, can't find the ')'");
                }

                // split the string to grab the identifier and remove the parentheses
                string returnStr = identifier.Substring(0, parenthesisStart);
                string parenthesis = identifier.Substring(parenthesisStart + 1, identifier.Length - returnStr.Length - 2);
                identifier = returnStr;

                parenthesisExpressions = ExtractOperationsParameters(parenthesis);
            }

            return identifier;
        }

        private static IDictionary<string, string> ExtractOperationsParameters(string identifier)
        {
            IDictionary<string, string> parenthesisExpressions = new Dictionary<string, string>();

            var items = identifier.Split(',');
            foreach (var item in items)
            {
                var subItems = item.Split('=');

                if (subItems.Length != 2)
                {
                    throw new Exception($"Invalid parameter at {item}");
                }

                parenthesisExpressions[subItems[0]] = subItems[1];
            }

            if (parenthesisExpressions.Any())
            {
                return parenthesisExpressions;
            }

            return null;
        }
    }
}
