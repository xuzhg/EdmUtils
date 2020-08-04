// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="FunctionImport">
    ///    <Key>
    ///      <PropertyRef Name = "Fullname" />
    ///    </ Key >
    ///    < Property Name="Fullname" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "IncludeInServiceDocument" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "Function" Type="Meta.Function" Nullable="false" Partner="FunctionImports" />
    ///    <NavigationProperty Name = "EntitySet" Type="Meta.EntitySet" />
    ///    <NavigationProperty Name = "EntityContainer" Type="Meta.EntityContainer" Nullable="false" Partner="FunctionImports" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>
    public class MetaFunctionImport : MetaElement
    {
        [Key]
        public string Fullname { get; set; }

        public string Name { get; set; }

        public bool IncludeInServiceDocument { get; set; }

        public MetaFunction Function { get; set; }

        public MetaEntitySet EntitySet { get; set; }
        public MetaEntityContainer EntityContainer { get; set; }

        public IList<MetaAnnotation> Annotations { get; set; }
    }
}