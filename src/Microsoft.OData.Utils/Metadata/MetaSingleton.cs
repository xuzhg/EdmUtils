// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="Singleton">
    ///    <Key>
    ///      <PropertyRef Name = "Fullname" />
    ///    </ Key >
    ///    < Property Name="Fullname" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <NavigationProperty Name = "Type" Type="Meta.EntityType" Nullable="false" />
    ///    <NavigationProperty Name = "NavigationPropertyBindings" Type="Collection(Meta.NavigationPropertyBinding)" Partner="Source" />
    ///    <NavigationProperty Name = "EntityContainer" Type="Meta.EntityContainer" Nullable="false" Partner="Singletons" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>

    public class MetaSingleton : MetaElement
    {

        [Key]
        public string Fullname { get; set; }

        public string Name { get; set; }
        public MetaEntityType Type { get; set; }

        public IList<MetaNavigationPropertyBinding> NavigationPropertyBindings { get; set; }

        public MetaEntityContainer EntityContainer { get; set; }

        public IList<MetaAnnotation> Annotations { get; set; }
    }
}