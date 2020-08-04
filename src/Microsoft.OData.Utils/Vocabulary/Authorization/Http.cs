// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Authorization
{
    /// <summary>
    /// Complex type 'Org.OData.Authorization.V1.Http'
    /// </summary>
    internal class Http : Authorization
    {
        /// <summary>
        /// HTTP Authorization scheme to be used in the Authorization header, as per RFC7235.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Format of the bearer token.
        /// </summary>
        public string BearerFormat { get; set; }

        /// <summary>
        /// Gets the security scheme type.
        /// </summary>
        public override SecuritySchemeType SchemeType => SecuritySchemeType.Http;
    }
}
