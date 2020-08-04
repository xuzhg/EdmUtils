// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Capabilities
{
    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.DeepUpdateSupportType
    /// </summary>
    // [Term("Org.OData.Capabilities.V1.DeepUpdateSupport")]
    internal class DeepUpdateSupportType
    {
        /// <summary>
        /// Gets Annotation target supports deep updates
        /// </summary>
        public bool? Supported { get; private set; }

        /// <summary>
        /// Gets Annotation target supports accepting and returning nested entities annotated with the `Core.ContentID` instance annotation.
        /// </summary>
        public bool? ContentIDSupported { get; private set; }
    }
}
