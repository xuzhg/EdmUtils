// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="ActionImport">
    ///    <Key>
    ///      <PropertyRef Name = "Fullname" />
    ///    </ Key >
    ///    < Property Name="Fullname" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <NavigationProperty Name = "Action" Type="Meta.Action" Nullable="false" Partner="ActionImports" />
    ///    <NavigationProperty Name = "EntitySet" Type="Meta.EntitySet" />
    ///    <NavigationProperty Name = "EntityContainer" Type="Meta.EntityContainer" Nullable="false" Partner="ActionImports" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>
    public class MetaActionImport : MetaElement
    {
        [Key]
        public string Fullname { get; set; }

        public string Name { get; set; }

        public MetaAction Action { get; set; }

        public MetaEntitySet EntitySet { get; set; }
        public MetaEntityContainer EntityContainer { get; set; }

        public IList<MetaAnnotation> Annotations { get; set; }
    }
}