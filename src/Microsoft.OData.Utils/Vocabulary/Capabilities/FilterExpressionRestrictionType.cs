// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Capabilities
{
    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.FilterExpressionRestrictionType
    /// </summary>
    internal class FilterExpressionRestrictionType
    {
        /// <summary>
        /// Gets the Path to the restricted property.
        /// </summary>
        public string Property { get; private set; }

        /// <summary>
        /// Gets the RequiresFilter value.
        /// <Property Name="AllowedExpressions" Type="Capabilities.FilterExpressionType">
        /// <TypeDefinition Name="FilterExpressionType" UnderlyingType="Edm.String">
        /// </summary>
        public string AllowedExpressions { get; private set; }
    }
}
