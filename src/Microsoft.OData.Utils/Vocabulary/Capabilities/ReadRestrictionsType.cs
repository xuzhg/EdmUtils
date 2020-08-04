// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Capabilities
{
    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.ReadRestrictionsBase
    /// </summary>
    internal abstract class ReadRestrictionsBase
    {
        /// <summary>
        /// Get the Entities can be retrieved.
        /// </summary>
        public bool? Readable { get; private set; }

        /// <summary>
        /// Gets the List of required scopes to invoke an action or function
        /// </summary>
        public IList<PermissionType> Permissions { get; private set; }

        /// <summary>
        /// Gets the Supported or required custom headers.
        /// </summary>
        public IList<CustomParameter> CustomHeaders { get; private set; }

        /// <summary>
        /// Gets the Supported or required custom query options.
        /// </summary>
        public IList<CustomParameter> CustomQueryOptions { get; private set; }

        /// <summary>
        /// Gets A brief description of the request.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets A lengthy description of the request.
        /// </summary>
        public string LongDescription { get; private set; }

        /// <summary>
        /// Test the target supports update.
        /// </summary>
        /// <returns>True/false.</returns>
        public bool IsReadable => Readable == null || Readable.Value;
    }

    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.ReadByKeyRestrictionsType
    /// Restrictions for retrieving an entity by key
    /// </summary>
    internal class ReadByKeyRestrictions : ReadRestrictionsBase
    {
        // nothing here
    }

    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.ReadRestrictionsType
    /// </summary>
    // [Term("Org.OData.Capabilities.V1.ReadRestrictions")]
    internal class ReadRestrictionsType : ReadRestrictionsBase
    {
        /// <summary>
        /// Gets the Restrictions for retrieving an entity by key
        /// </summary>
        public ReadByKeyRestrictions ReadByKeyRestrictions { get; set; }
    }
}
