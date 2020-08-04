// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <ComplexType Name="Parameter">
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "IsBinding" Type="Edm.Boolean" Nullable="false" />
    ///    <Property Name = "Nullable" Type="Edm.Boolean" Nullable="false" />
    ///    <Property Name = "Facets" Type="Collection(Meta.Facet)" />
    ///    <Property Name = "IsCollection" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "Type" Type="Meta.Type" Nullable="false" />
    ///    <Property Name = "Annotations" Type="Collection(Meta.InlineAnnotation)" />
    ///  </ComplexType>
    /// </summary>
    public class MetaParameter : MetaElement
    {
        public string Name { get; set; }

        public bool IsBinding { get; set; }

        public bool Nullable { get; set; }

        public IList<MetaFacet> Facets { get; set; }

        public bool IsCollection { get; set; }


        public MetaType Type { get; set; }

        public IList<MetaInlineAnnotation> Annotations { get; set; }

    }
}