// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.EdmUtils;
using Microsoft.OData.EdmUtils.Segments;
using Newtonsoft.Json.Linq;

namespace AnnotationGenerator.MD
{
    public class ApiPermissionItem
    {
        public ApiPermissionItem(string requestUri, IList<ApiPermissionType> permissions)
        {
            RequestUri = requestUri;
            Permissions = permissions;
        }

        public string RequestUri { get; }

        public IList<ApiPermissionType> Permissions { get; }

        public bool IsValid => RequestPath != null;

        public UriPath RequestPath { get; private set; }

        public string PaserError { get; private set; }

        public void ProcessByEdmModel(IEdmModel model, PathParserSettings settings)
        {
            RequestPath = null;
            try
            {
                RequestPath = PathParser.ParsePath(RequestUri, model, settings);
            }
            catch (Exception innerEx)
            {
                PaserError = innerEx.Message;
                RequestPath = null;
            }
        }
    }

    public class PermissionSchemeItem
    {

    }

    /// <summary>
    /// {
    ///   "ApiPermissions": {
    ///      "/security/alerts/{alert_id}": [
    ///        {
    ///         "HttpVerb": "GET",
    ///         "DelegatedWork": [
    ///         ......
    ///         }
    ///       ]
    ///    },
    ///    "PermissionSchemes": {
    ///       "DelegatedWork": [
    ///         {
    ///           "Name": "AccessReview.Read.All",
    ///           "Description": "Allows the app to read access reviews on behalf of the signed-in user.",
    ///           "Grant": "admin"
    ///         },
    ///         {
    ///           .....
    ///    }
    /// </summary>
    public class ApiPermissionsWrapper
    {
        public IDictionary<string, IList<ApiPermissionType>> ApiPermissions { get; private set; }

        public IDictionary<string, IList<ApiPermissionsBySchemeType>> PermissionsByScheme { get; private set; }

        public IDictionary<string, Exception> UriParserError { get; private set; } = new Dictionary<string, Exception>();

        public IList<string> MergedRequests { get; } = new List<string>();

        public IDictionary<UriPath, IList<ApiPermissionType>> ApiPermissionsProcessed { get; private set; }

        /// <summary>
        /// Load all Permissions, it's a JSON object, includes two properties;
        /// {
        ///     "ApiPermissions" : {}
        ///     "PermissionSchemes" {}
        /// }
        /// </summary>
        /// <param name="fileName">The permissions files.</param>
        /// <returns>the ApiPermissionsWrapper</returns>
        public static ApiPermissionsWrapper LoadAll(string fileName)
        {
            ApiPermissionsWrapper wrapper = new ApiPermissionsWrapper();
            try
            {
                string json = File.ReadAllText(fileName);
                JObject jObj = JObject.Parse(json);

                JProperty property = jObj.Property("ApiPermissions");
                if (property != null)
                {
                    // ApiPermissions
                    wrapper.ApiPermissions = LoadTopLevelProperty<ApiPermissionType>(jObj, "ApiPermissions");

                    // PermissionSchemes
                    wrapper.PermissionsByScheme = LoadTopLevelProperty<ApiPermissionsBySchemeType>(jObj, "PermissionSchemes");
                }
                else
                {
                    wrapper.ApiPermissions = LoadPermissions(jObj);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return wrapper;
        }

        /// <summary>
        /// Process the permissions by the IEdmModel.
        /// </summary>
        /// <param name="model">The Edm Model.</param>
        public void Process(IEdmModel model)
        {
            IDictionary<UriPath, IList<ApiPermissionType>> processed = new Dictionary<UriPath, IList<ApiPermissionType>>(new UriPathEqualityComparer());
            PathParserSettings settings = new PathParserSettings
            {
                EnableCaseInsensitive = true
            };

            foreach (var permission in ApiPermissions)
            {
                // Do Uri parser & save the invalid Uri into dictionary
                UriPath path;
                try
                {
                    path = PathParser.ParsePath(permission.Key, model, settings);
                }
                catch (Exception innerEx)
                {
                    UriParserError[permission.Key] = innerEx;
                    path = null;
                }

                if (path == null)
                {
                    continue;
                }

                if (processed.TryGetValue(path, out IList<ApiPermissionType> value))
                {
                    MergePermissions(value, permission.Value);
                    MergedRequests.Add(permission.Key);
                }
                else
                {
                    processed[path] = permission.Value;
                }
            }

            int index = 0;
            // for each property, navigation property segment
            foreach (var item in processed)
            {
                index++;

                PathSegment lastSegment = item.Key.LastSegment;
                if (lastSegment.Kind == SegmentKind.Property || lastSegment.Kind == SegmentKind.Navigation)
                {
                    // only process the property
                    TryFindPreviousPath(item, processed);
                }
            }

            ApiPermissionsProcessed = processed;
        }

        private void TryFindPreviousPath(KeyValuePair<UriPath, IList<ApiPermissionType>> loopUp, IDictionary<UriPath, IList<ApiPermissionType>> processed)
        {
            UriPath path = loopUp.Key;
            int count = path.Count;
            IList<PathSegment> segments = new List<PathSegment>();
            int skip = 2;
            if (path.Segments[path.Count - 1].Kind != SegmentKind.Key)
            {
                skip = 1;
            }

            for (int i = 0; i < count - skip; i++)
            {
                // get rid of the "last key segment and last property (nav) segment)
                segments.Add(path.Segments[i]);
            }

            UriPath newPath = new UriPath(segments);

            string propertyName = loopUp.Key.LastSegment.UriLiteral;

            // find all target string is same as the loop target string (prefix)
            foreach (var foundItem in processed.Where(p => p.Key.EqualsTo(newPath)))
            {
                TryFindPreviousPath(foundItem, loopUp, propertyName);
            }
        }

        private void TryFindPreviousPath(KeyValuePair<UriPath, IList<ApiPermissionType>> append,
            KeyValuePair<UriPath, IList<ApiPermissionType>> loopUp, string propertyName)
        {
            foreach(var item in loopUp.Value)
            {
                var sameHttpVerbs = append.Value.Where(a => a.HttpVerb == item.HttpVerb).ToList();
                if (sameHttpVerbs.Count > 1)
                {
                    // throw new Exception("TODO: Find mulitple Http Verbs.");
                    Console.WriteLine("TODO: Find mulitple Http Verbs.");
                }
                else if (sameHttpVerbs.Count == 1)
                {
                    AppendTheRestrictedProperty(sameHttpVerbs[0], item, propertyName);
                }
            }
        }

        private void AppendTheRestrictedProperty(ApiPermissionType append, ApiPermissionType loopUp, string propertyName)
        {
            foreach(var scope in append.DelegatedWork)
            {
                if (!loopUp.DelegatedWork.Any(a => a == scope))
                {
                    HashSet<string> restricted;
                    if (!append.DelegatedWorkRestrictedProperties.TryGetValue(scope, out restricted))
                    {
                        restricted = new HashSet<string>();
                        append.DelegatedWorkRestrictedProperties[scope] = restricted;
                    }
                    restricted.Add(propertyName);
                }
            }

            foreach(var scope in append.DelegatedPersonal)
            {
                if (!loopUp.DelegatedPersonal.Any(a => a == scope))
                {
                    HashSet<string> restricted;
                    if (!append.DelegatedPersonalRestrictedProperties.TryGetValue(scope, out restricted))
                    {
                        restricted = new HashSet<string>();
                        append.DelegatedPersonalRestrictedProperties[scope] = restricted;
                    }
                    restricted.Add(propertyName);
                }
            }

            foreach (var scope in append.Application)
            {
                if (!loopUp.Application.Any(a => a == scope))
                {
                    HashSet<string> restricted;
                    if (!append.ApplicationRestrictedProperties.TryGetValue(scope, out restricted))
                    {
                        restricted = new HashSet<string>();
                        append.ApplicationRestrictedProperties[scope] = restricted;
                    }
                    restricted.Add(propertyName);
                }
            }
        }

        private static void MergePermissions(IList<ApiPermissionType> source, IList<ApiPermissionType> newPermissions)
        {
            foreach (var permType in newPermissions)
            {
                ApiPermissionType sameHttpVerbPermission = source.FirstOrDefault(s => s.HttpVerb == permType.HttpVerb);
                if (sameHttpVerbPermission != null)
                {
                    sameHttpVerbPermission.DelegatedWork = sameHttpVerbPermission.DelegatedWork.Union(permType.DelegatedWork).ToList();
                    sameHttpVerbPermission.DelegatedPersonal = sameHttpVerbPermission.DelegatedPersonal.Union(permType.DelegatedPersonal).ToList();
                    sameHttpVerbPermission.Application = sameHttpVerbPermission.Application.Union(permType.Application).ToList();
                }
                else
                {
                    source.Add(permType);
                }
            }
        }

        private static IDictionary<string, IList<T>> LoadTopLevelProperty<T>(JObject topLevelObject, string propertyName)
        {
            JProperty property = topLevelObject.Property(propertyName);
            if (property == null)
            {
                throw new Exception($"Invalid format, Need a top level property named '{propertyName}'.");
            }

            JObject propertyValue = property.Value as JObject;
            if (propertyValue == null)
            {
                throw new Exception($"Invalid format, Need an object value of the property: '{propertyName}'.");
            }

            var dict = new Dictionary<string, IList<T>>();
            foreach (var subProperty in propertyValue.Properties())
            {
                JArray array = subProperty.Value as JArray;
                if (array == null)
                {
                    throw new Exception($"Invalid format, Need an array value of the property: {subProperty.Name}");
                }

                IList<T> subPropertyList = new List<T>();
                foreach (var item in array)
                {
                    JObject subPermissionsBySchemeObj = item as JObject;
                    if (subPermissionsBySchemeObj == null)
                    {
                        throw new Exception($"Not valid format, Need object value of array in the property: {property.Name}");
                    }

                    T subPermObject = subPermissionsBySchemeObj.ToObject<T>();
                    subPropertyList.Add(subPermObject);
                }

                dict[subProperty.Name.Trim()] = subPropertyList;
            }

            return dict;
        }

        private static IDictionary<string, IList<ApiPermissionType>> LoadPermissions(JObject topLevelObject)
        {
            IDictionary<string, IList<ApiPermissionType>> returns =
                new Dictionary<string, IList<ApiPermissionType>>();
            foreach (var uriProperty in topLevelObject.Properties())
            {
                JObject uriPropertyValue = uriProperty.Value as JObject;

                if (uriPropertyValue == null)
                {
                    throw new Exception($"Invalid format, Need a JSON Object at '{uriProperty.Name}'.");
                }

                IList<ApiPermissionType> permissions = new List<ApiPermissionType>();
                foreach (var httpMethodProperty in uriPropertyValue.Properties())
                {
                    ApiPermissionType permission = new ApiPermissionType
                    {
                        HttpVerb = httpMethodProperty.Name
                    };

                    JObject httpMethodPropertyValue = httpMethodProperty.Value as JObject;
                    if (httpMethodPropertyValue == null)
                    {
                        throw new Exception($"Invalid format, Need a JSON Object at '{uriProperty.Name}'\'{httpMethodProperty.Name}.");
                    }

                    foreach (var permissionProperty in httpMethodPropertyValue.Properties())
                    {
                        JArray permissionPropertyValue = permissionProperty.Value as JArray;
                        if (permissionPropertyValue == null)
                        {
                            throw new Exception($"Invalid format, Need an array at '{uriProperty.Name}'\'{httpMethodProperty.Name}.");
                        }

                        if (permissionProperty.Name == "Application")
                        {
                            permission.Application = permissionPropertyValue.ToObject<List<string>>();
                        }
                        else if (permissionProperty.Name == "DelegatedWork")
                        {
                            permission.DelegatedWork = permissionPropertyValue.ToObject<List<string>>();
                        }
                        else if (permissionProperty.Name == "DelegatedPersonal")
                        {
                            permission.DelegatedPersonal = permissionPropertyValue.ToObject<List<string>>();
                        }
                        else
                        {
                            throw new Exception($"Invalid {permissionProperty.Name} at '{uriProperty.Name}'\'{httpMethodProperty.Name}.");
                        }
                    }

                    permissions.Add(permission);
                }

                returns[uriProperty.Name] = permissions;
            }

            return returns;
        }
    }
}
