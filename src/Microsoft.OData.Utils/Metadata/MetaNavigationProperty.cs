// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="NavigationProperty">
    ///    <Key>
    ///      <PropertyRef Name = "Fullname" />
    ///    </ Key >
    ///    < Property Name="Fullname" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Nullable" Type="Edm.Boolean" Nullable="false" />
    ///    <Property Name = "ContainsTarget" Type="Edm.Boolean" Nullable="false" />
    ///    <Property Name = "OnDelete" Type="Meta.Include" />
    ///    <Property Name = "ReferentialConstraints" Type="Collection(Meta.ReferentialConstraint)" />
    ///    <Property Name = "IsCollection" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "Type" Type="Meta.EntityType" Nullable="false" />
    ///    <NavigationProperty Name = "Partner" Type="Meta.NavigationProperty" />
    ///    <NavigationProperty Name = "NavigationPropertyBindings" Type="Collection(Meta.NavigationPropertyBinding)" Partner="NavigationProperty" />
    ///    <NavigationProperty Name = "DeclaringType" Type="Meta.StructuredType" Nullable="false" Partner="NavigationProperties" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>
    public class MetaNavigationProperty : MetaElement
    {
        [Key]
        public string Fullname { get; set; }

        public string Name { get; set; }

        public bool Nullable { get; set; }

        public bool ContainsTarget { get; set; }

        public MetaOnDelete OnDelete { get; set; }

        public IList<MetaReferentialConstraint> ReferentialConstraints { get; set; }

        public bool IsCollection { get; set; }

        public MetaType Type { get; set; }

        public MetaNavigationProperty Partner { get; set; }

        public IList<MetaNavigationPropertyBinding> NavigationPropertyBindings { get; set; }


        public MetaStructuredType DeclaringType { get; set; }

        public IList<MetaAnnotation> Annotations { get; set; }
    }
}