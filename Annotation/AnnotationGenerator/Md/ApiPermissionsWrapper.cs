// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;

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
    }
}
