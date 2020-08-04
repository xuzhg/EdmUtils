// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <ComplexType Name="ReturnType">
    ///    <Property Name = "Nullable" Type="Edm.Boolean" Nullable="false" />
    ///    <Property Name = "Facets" Type="Collection(Meta.Facet)" />
    ///    <Property Name = "IsCollection" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "Type" Type="Meta.Type" Nullable="false" />
    ///  </ComplexType>
    /// </summary>
    public class MetaReturnType : MetaElement
    {
        public bool Nullable { get; set; }
        public IList<MetaFacet> Facets { get; set; }
        public bool IsCollection { get; set; }

        public MetaType Type { get; set; }
    }
}