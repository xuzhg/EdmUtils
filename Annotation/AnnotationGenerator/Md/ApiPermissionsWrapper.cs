// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Annotation;
using Annotation.EdmUtil;
using AnnotationGenerator.Vocabulary;
using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AnnotationGenerator.MD
{
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
    ///    "PermissionsByScheme": {
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
        public IDictionary<string, IList<ApiPermissionType>> ApiPermissions { get; set; }

        public IDictionary<string, IList<ApiPermissionsBySchemeType>> PermissionsByScheme { get; set; }

        public IDictionary<string, Exception> UriParserError { get; set; } = new Dictionary<string, Exception>();

        public IDictionary<UriPath, IList<ApiPermissionType>> ApiPermissionsProcessed { get; set; }

        public void Process(IEdmModel model)
        {
            IDictionary<UriPath, IList<ApiPermissionType>> processed = new Dictionary<UriPath, IList<ApiPermissionType>>();

            foreach (var permission in ApiPermissions)
            {
                // Do Uri parser & save the invalid Uri into dictionary
                var path = ParseRequestUri(permission.Key, model);
                if (path == null)
                {
                    continue;
                }

                processed[path] = permission.Value;
            }

            int index = 0;
            // for each property segment
            foreach (var item in processed)
            {
                index++;

                // only process the property
                if (item.Key.Kind != PathKind.Property)
                {
                    continue;
                }
                TryFindPreviousPath(item, processed);
            }

            ApiPermissionsProcessed = processed;
        }

        private void TryFindPreviousPath(KeyValuePair<UriPath, IList<ApiPermissionType>> loopUp, IDictionary<UriPath, IList<ApiPermissionType>> processed)
        {
            string loopTarget = loopUp.Key.TargetString;
            int start = loopTarget.LastIndexOf('/'); // ~/users/mailsettings
            loopTarget = loopTarget.Substring(0, start); // ~/users

            string propertyName = loopUp.Key.LastSegment.Identifier;

            // find all target string is same as the loop target string (prefix)
            var found = processed.Where(p => p.Key.GetTargetString() == loopTarget).ToList();
            foreach(var foundItem in found)
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
                    throw new Exception("TODO: Find mulitple Http Verbs.");
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
                if (!loopUp.DelegatedWork.Any(a => a.ScopeName == scope.ScopeName))
                {
                    scope.RestrictedProperties.Add(propertyName);
                }
            }

            foreach(var scope in append.DelegatedPersonal)
            {
                if (!loopUp.DelegatedPersonal.Any(a => a.ScopeName == scope.ScopeName))
                {
                    scope.RestrictedProperties.Add(propertyName);
                }
            }

            foreach (var scope in append.Application)
            {
                if (!loopUp.Application.Any(a => a.ScopeName == scope.ScopeName))
                {
                    scope.RestrictedProperties.Add(propertyName);
                }
            }
        }

        public UriPath ParseRequestUri(string requestUri, IEdmModel model)
        {
            UriPath path;
            try
            {
                path = PathParser.ParsePath(requestUri, model);
            }
            catch
            {
                try
                {
                    path = PathParser.ParsePath(requestUri, model, true);
                }
                catch (Exception innerEx)
                {
                    UriParserError[requestUri] = innerEx;
                    return null;
                }
            }

            return path;
        }
    }
}
