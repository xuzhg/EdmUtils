// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="NavigationPropertyBinding">
    ///    <Key>
    ///      <PropertyRef Name = "Fullname" />
    ///    </ Key >
    ///    < Property Name="Fullname" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Path" Type="Edm.String" Nullable="false" />
    ///    <NavigationProperty Name = "Target" Type="Edm.EntityType" Nullable="false" />
    ///    <NavigationProperty Name = "Source" Type="Edm.EntityType" Nullable="false" />
    ///    <NavigationProperty Name = "NavigationProperty" Type="Meta.NavigationProperty" Nullable="false" Partner="NavigationPropertyBindings" />
    ///  </EntityType>
    /// </summary>
    public class MetaNavigationPropertyBinding : MetaElement
    {
        [Key]
        public string Fullname { get; set; }

        public string Path { get; set; }

        public MetaEntityType Target { get; set; }

        public MetaEntityType Source { get; set; }

        public MetaNavigationProperty NavigationProperty { get; set; }

    }
}