// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Authorization
{
    /// <summary>
    /// OAuth2 type kind.
    /// </summary>
    internal enum OAuth2Type
    {
        /// <summary>
        /// ClientCredentials
        /// </summary>
        ClientCredentials,

        /// <summary>
        /// Implicit
        /// </summary>
        Implicit,

        /// <summary>
        /// Pasword
        /// </summary>
        Pasword,

        /// <summary>
        /// AuthCode
        /// </summary>
        AuthCode
    }

    /// <summary>
    /// Abstract complex type 'Org.OData.Authorization.V1.OAuthAuthorization'
    /// </summary>
    internal abstract class OAuthAuthorization : Authorization
    {
        /// <summary>
        /// Available scopes.
        /// </summary>
        public IList<AuthorizationScope> Scopes { get; set; }

        /// <summary>
        /// Refresh Url
        /// </summary>
        public string RefreshUrl { get; set; }

        /// <summary>
        /// Gets the security scheme type.
        /// </summary>
        public override SecuritySchemeType SchemeType => SecuritySchemeType.OAuth2;

        /// <summary>
        /// Gets the OAuth2 type.
        /// </summary>
        public abstract OAuth2Type OAuth2Type { get; }
    }
}