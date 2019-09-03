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

        public IList<string> DelegatedWork { get; set; }

        public IList<string> DelegatedPersonal { get; set; }

        public IList<string> Application { get; set; }
    }
}
