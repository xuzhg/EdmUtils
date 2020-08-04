// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Authorization
{
    /// <summary>
    /// Complex type: 'Org.OData.Authorization.V1.AuthorizationScope'
    /// </summary>
    internal class AuthorizationScope
    {
        /// <summary>
        /// Scope name.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Scope Grant.
        /// Identity that has access to the scope or can grant access to the scope.
        /// </summary>
        public string Grant { get; set; }

        /// <summary>
        /// Description of the scope.
        /// </summary>
        public string Description { get; set; }
    }
}
