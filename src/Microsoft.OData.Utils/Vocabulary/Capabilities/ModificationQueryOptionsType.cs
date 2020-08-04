// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Capabilities
{
    /// <summary>
    /// Complex type: Org.OData.Capabilities.V1.ModificationQueryOptionsType
    /// </summary>
    // [Term("Org.OData.Capabilities.V1.ModificationQueryOptions")]
    internal class ModificationQueryOptionsType
    {
        /// <summary>
        /// Gets/sets the $expand with modification requests.
        /// </summary>
        public bool? ExpandSupported { get; private set; }

        /// <summary>
        /// Gets/sets the $select with modification requests.
        /// </summary>
        public bool? SelectSupported { get; private set; }

        /// <summary>
        /// Gets/sets the $compute with modification requests.
        /// </summary>
        public bool? ComputeSupported { get; private set; }

        /// <summary>
        /// Gets/sets the $filter with modification requests.
        /// </summary>
        public bool? FilterSupported { get; private set; }

        /// <summary>
        /// Gets/sets the $search with modification requests.
        /// </summary>
        public bool? SearchSupported { get; private set; }

        /// <summary>
        /// Gets/sets the $sort with modification requests.
        /// </summary>
        public bool? SortSupported { get; private set; }
    }
}
