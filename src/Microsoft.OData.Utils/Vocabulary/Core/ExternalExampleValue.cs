// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Utils.Vocabulary.Core
{
    /// <summary>
    /// Complex type: Org.OData.Core.V1.ExternalExampleValue.
    /// </summary>
    internal class ExternalExampleValue : ExampleValue
    {
        /// <summary>
        /// Gets the Url reference to the value in its literal format
        /// </summary>
        public string ExternalValue { get; set; }
    }
}
