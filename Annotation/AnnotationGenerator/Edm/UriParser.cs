
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.OData.Edm;

namespace AnnotationGenerator.Edm
{
    public class UriParser
    {
        public static IList<PathSegment> Parse(string requestUri, IEdmModel model)
        {
            var items = requestUri.Split("/");

            IList<PathSegment> segments = new List<PathSegment>();
            PathSegment currSegment = null;
            foreach (var item in items)
            {
                if (String.IsNullOrEmpty(item))
                {
                    continue;
                }

                if (currSegment == null)
                {
                    currSegment = CreateFirstSegment(item, model);
                }
                else
                {
                    currSegment = CreateNextSegment(currSegment, item, model);
                }

                if (currSegment == null)
                {
                    throw new Exception($"Unknown segment {item} in the path {requestUri}");
                }

                segments.Add(currSegment);
            }

            return segments;
        }

        private static PathSegment CreateFirstSegment(string identifier, IEdmModel model)
        {
            // We only process the singleton/entityset/operationimport
            // the identifier maybe include the key, for example: ~/users({id})
            string parenthesisExpressions;
            identifier = ExtractParenthesis(identifier, out parenthesisExpressions);

            IEdmEntitySet entitySet = model.FindDeclaredEntitySet(identifier);
            if (entitySet != null)
            {
                EntitySetSegment entitySetSegment = new EntitySetSegment(entitySet);
                TryBindKeySegment(entitySetSegment, parenthesisExpressions);
                return entitySetSegment;
            }

            IEdmSingleton singleton = model.FindDeclaredSingleton(identifier);
            if (singleton != null)
            {
                return new SingletonSegment(singleton);
            }

            throw new Exception($"Unknow type of first segment: {identifier}");

            // TODO: support operationImport?
            // var operationImports = model.FindDeclaredOperationImports(identifier);
        }

        private static PathSegment CreateNextSegment(PathSegment preSegment, string identifier, IEdmModel model)
        {
            // GET /Users/{id}
            // GET /Users({id})
            // GET /me/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'{timezone_format}')
            string parenthesisExpressions;

            // maybe key or function parameters
            identifier = ExtractParenthesis(identifier, out parenthesisExpressions);

            if (preSegment.IsSingle)
            {
                // can be "property, navproperty, bound operations"
                IEdmProperty property;
                if (TryBindProperty(preSegment, identifier, out property))
                {

                }
            }

            // type cast, 
            if (identifier.IndexOf('.') >= 0)
            {
                if (TryBindTypeCast(preSegment, identifier))
                {
                    // return TypeCastSegment;
                }
            }

            // bound operations
            if (TryBindOperations(preSegment, identifier, parenthesisExpressions))
            {

            }

            // Handle Key As Segment
            if (TryBindKeySegment(preSegment, identifier))
            {

            }

            throw new Exception($"Unknow type of first segment: {identifier}");
        }

        private static bool TryBindKeySegment(PathSegment preSegment, string parenthesisExpressions)
        {
            if (preSegment.IsSingle)
            {
                return false;
            }

            IEdmEntityType targetEntityType;
            if (preSegment.EdmType == null || !preSegment.EdmType.IsEntityOrEntityCollectionType(out targetEntityType))
            {
                return false;
            }

            parenthesisExpressions.ExtractKeyValuePairs(out IDictionary<string, string> keys);
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

            preSegment.NestedKeySegment = new KeySegment(keys, preSegment);
            return true;
        }

        private static bool TryBindTypeCast(PathSegment preSegment, string identifier)
        {
            return true;
        }

        private static bool TryBindOperations(PathSegment preSegment, string identifier, string parenthesisExpressions)
        {
            IEdmType bindingType = preSegment.EdmType;


            // TODO: do we need to process the ~/.../NS.Function(p1={abc})({id})
            // TODO: do we need to process the ~/.../NS.Function(p1={abc})/{id}
            return true;
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

        private static string ExtractParenthesis(string identifier, out string parenthesisExpressions)
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
                parenthesisExpressions = identifier.Substring(parenthesisStart + 1, identifier.Length - returnStr.Length - 2);
                identifier = returnStr;
            }

            return identifier;
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

            var items = identifier.Split(",");
            foreach (var item in items)
            {
                var subItems = item.Split("=");

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
