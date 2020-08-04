// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="EnumTypeMember">
    ///    <Key>
    ///      <PropertyRef Name = "Fullname" />
    ///    </ Key >
    ///    < Property Name="Fullname" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Value" Type="Edm.Int64" Nullable="false" />
    ///    <NavigationProperty Name = "EnumType" Type="Meta.EnumType" Nullable="false" Partner="Members" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>
    public class MetaEnumTypeMember : MetaElement
    {
        [Key]
        public string Fullname { get; set; }

        public string Name { get; set; }

        public long Value { get; set; }

        public MetaEnumType EnumType { get; set; }

        public List<MetaAnnotation> Annotations { get; set; }
    }
}