// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <ComplexType Name="ReferentialConstraint">
    ///    <Property Name = "Property" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "ReferencedProperty" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Annotations" Type="Collection(Meta.InlineAnnotation)" />
    ///  </ComplexType>
    /// </summary>
    public class MetaReferentialConstraint : MetaElement
    {
        public string Property { get; set; }

        public string ReferencedProperty { get; set; }

        public IList<MetaInlineAnnotation> Annotations { get; set; }
    }
}