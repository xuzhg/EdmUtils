// -----------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Authorization
{
    //
    // Summary:
    //     The type of the security scheme
    public enum SecuritySchemeType
    {
        //
        // Summary:
        //     Use API key
        ApiKey = 0,
        //
        // Summary:
        //     Use basic or bearer token authorization header.
        Http = 1,
        //
        // Summary:
        //     Use OAuth2
        OAuth2 = 2,
        //
        // Summary:
        //     Use OAuth2 with OpenId Connect URL to discover OAuth2 configuration value.
        OpenIdConnect = 3
    }

    /// <summary>
    /// Complex type: Org.OData.Authorization.V1.SecurityScheme
    /// </summary>
    // [Term("Org.OData.Authorization.V1.SecuritySchemes")]
    internal class SecurityScheme
    {
        /// <summary>
        /// The name of a required authorization scheme.
        /// </summary>
        public string Authorization { get; set; }

        /// <summary>
        /// The names of scopes required from this authorization scheme.
        /// </summary>
        public IList<string> RequiredScopes { get; set; }
    }
}