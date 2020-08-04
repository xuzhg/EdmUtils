// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Authorization
{
    /// <summary>
    /// Enum type KeyLocation
    /// </summary>
    internal enum KeyLocation
    {
        /// <summary>
        /// API Key is passed in the header.
        /// </summary>
        Header,

        /// <summary>
        /// API Key is passed as a query option.
        /// </summary>
        QueryOption,

        /// <summary>
        /// API Key is passed as a cookie.
        /// </summary>
        Cookie
    }

    /// <summary>
    /// Complex type: 'Org.OData.Authorization.V1.ApiKey'
    /// </summary>
    internal class ApiKey : Authorization
    {
        /// <summary>
        /// The name of the header or query parameter.
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Whether the API Key is passed in the header or as a query option.
        /// </summary>
        public KeyLocation? Location { get; set; }

        /// <summary>
        /// Gets the security scheme type.
        /// </summary>
        public override SecuritySchemeType SchemeType => SecuritySchemeType.ApiKey;
    }
}