// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.OData.EdmUtils
{
    /// <summary>
    /// Represents the kind of a OData path.
    /// </summary>
    public enum PathKind
    {
        /// <summary>
        /// Entity set, for example: ~/users
        /// </summary>
        EntitySet,

        /// <summary>
        /// Singleton, for example: ~/me
        /// </summary>
        Singleton,

        /// <summary>
        /// Single entity, for example: ~/users({id})
        /// </summary>
        Entity,

        /// <summary>
        /// Single navigation, for example: ~/users({id})/contact
        /// </summary>
        SingleNavigation,

        /// <summary>
        /// Collection navigation, for example: ~/users({id})/contacts
        /// </summary>
        CollectionNavigation,

        /// <summary>
        /// Property access, for example: ~/users({id})/name
        /// </summary>
        Property,

        /// <summary>
        /// Bound operation (function/action), for example: ~/users({id})/ms.graph.getById
        /// </summary>
        Operation,

        /// <summary>
        /// Unbound operation (function/action), for example: ~/resetDatabase(...)
        /// </summary>
        OperationImport,

        /// <summary>
        /// Type cast
        /// </summary>
        TypeCast
    }
}
