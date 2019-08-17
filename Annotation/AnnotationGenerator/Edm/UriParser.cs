using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

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
            IEdmEntitySet entitySet = model.FindDeclaredEntitySet(identifier);
            if (entitySet != null)
            {
                return new EntitySetSegment(entitySet);
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
            // GET /me/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'{timezone_format}')

            IDictionary<string, string> parenthesisExpressions;
            identifier = ExtractOperations(identifier, out parenthesisExpressions);

            if (preSegment.IsSingle)
            {
                // can be "type cast, property, navproperty, bound operations"
            }
            else
            {
                // can be type cast, key, bound operations.
            }
        }

        private static string ExtractOperations(string identifier, out IDictionary<string, string> parenthesisExpressions)
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
