// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    ///  <ComplexType Name="Facet">
    ///    <Property Name = "Name" Type="Meta.FacetName" Nullable="false" />
    ///    <Property Name = "Value" Type="Edm.String" Nullable="false" />
    ///  </ComplexType>
    /// </summary>
    public class MetaFacet : MetaElement
    {
        public MetaFacetName Name { get; set; }

        public string Value { get; set; }
    }
}