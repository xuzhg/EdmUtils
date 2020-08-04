// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="Property">
    ///    <Key>
    ///      <PropertyRef Name = "Fullname" />
    ///    </ Key >
    ///    < Property Name="Fullname" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Nullable" Type="Edm.Boolean" Nullable="false" />
    ///    <Property Name = "DefaultValue" Type="Edm.String" />
    ///    <Property Name = "Facets" Type="Collection(Meta.Facet)" />
    ///    <Property Name = "IsCollection" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "Type" Type="Meta.Type" Nullable="false" />
    ///    <NavigationProperty Name = "DeclaringType" Type="Meta.StructuredType" Nullable="false" Partner="Properties" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>
    public class MetaProperty : MetaElement
    {
        [Key]
        public string Fullname { get; set; }

        public string Name { get; set; }

        public bool Nullable { get; set; }

        public string DefaultValue { get; set; }

        public IList<MetaFacet> Facets { get; set; }

        public bool IsCollection { get; set; }

        public MetaType Type { get; set; }

        public MetaStructuredType DeclaringType { get; set; }

        public IList<MetaAnnotation> Annotations { get; set; }
    }
}