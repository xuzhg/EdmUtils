// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AnnotationGenerator.MD
{
    /// <summary>
    /// {
    ///    "HttpVerb": "GET",
    ///    "DelegatedWork": [
    ///      "SecurityEvents.Read.All",
    ///      " SecurityEvents.ReadWrite.All"
    ///    ],
    ///    "DelegatedPersonal": [
    ///      "Not supported."
    ///    ],
    ///    "Application": [
    ///      "SecurityEvents.Read.All",
    ///      " SecurityEvents.ReadWrite.All"
    ///    ]
    /// }
    /// </summary>
    public class ApiPermissionType
    {
        public string HttpVerb { get; set; }

        public IList<PermissionScopeType> DelegatedWork { get; set; }

        public IList<PermissionScopeType> DelegatedPersonal { get; set; }

        public IList<PermissionScopeType> Application { get; set; }
    }

    public class PermissionScopeType
    {
        public string ScopeName { get; set; }

        public HashSet<string> RestrictedProperties { get; set; } = new HashSet<string>();
    }
}
