// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;

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

        public IList<string> DelegatedWork { get; set; } = new List<string>();

        public IList<string> DelegatedPersonal { get; set; } = new List<string>();

        public IList<string> Application { get; set; } = new List<string>();

        public IDictionary<string, HashSet<string>> DelegatedWorkRestrictedProperties { get; set; } = new Dictionary<string, HashSet<string>>();

        public IDictionary<string, HashSet<string>> DelegatedPersonalRestrictedProperties { get; set; } = new Dictionary<string, HashSet<string>>();

        public IDictionary<string, HashSet<string>> ApplicationRestrictedProperties { get; set; } = new Dictionary<string, HashSet<string>>();
    }

    public class PermissionScopeType
    {
        public string ScopeName { get; set; }

        public HashSet<string> RestrictedProperties { get; set; } = new HashSet<string>();
    }
}
