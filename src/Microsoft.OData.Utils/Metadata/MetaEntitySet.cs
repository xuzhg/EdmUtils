// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    ///  <EntityType Name="EntitySet">
    ///    <Key>
    ///      <PropertyRef Name = "Fullname" />
    ///    </ Key >
    ///    < Property Name="Fullname" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "IncludeInServiceDocument" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "EntityType" Type="Meta.EntityType" Nullable="false" Partner="EntitySets" />
    ///    <NavigationProperty Name = "NavigationPropertyBindings" Type="Collection(Meta.NavigationPropertyBinding)" Partner="Source" />
    ///    <NavigationProperty Name = "EntityContainer" Type="Meta.EntityContainer" Nullable="false" Partner="EntitySets" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>
    public class MetaEntitySet : MetaElement
    {
        [Key]
        public string Fullname { get; set; }

        public string Name { get; set; }

        public bool IncludeInServiceDocument { get; set; }

        public MetaEntityType EntityType { get; set; }

        public IList<MetaNavigationPropertyBinding> NavigationPropertyBindings { get; set; }

        public MetaEntityContainer EntityContainer { get; set; }

        public IList<MetaAnnotation> Annotations { get; set; }

    }
}