// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="Term">
    ///    <Key>
    ///      <PropertyRef Name = "QualifiedName" />
    ///    </ Key >
    ///    < Property Name="QualifiedName" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "DefaultValue" Type="Edm.String" />
    ///    <Property Name = "IsCollection" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "Type" Type="Meta.Type" Nullable="false" />
    ///    <NavigationProperty Name = "BaseTerm" Type="Meta.Term" />
    ///    <NavigationProperty Name = "Applications" Type="Collection(Meta.Annotation)" Partner="Term" />
    ///    <NavigationProperty Name = "Schema" Type="Meta.Schema" Nullable="false" Partner="Terms" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>
    public class MetaTerm : MetaElement
    {
        [Key]
        public string QualifiedName { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
        public bool IsCollection { get; set; }
        public MetaType Type { get; set; }
        public MetaTerm BaseTerm { get; set; }
        public IList<MetaAnnotation> Applications { get; set; }
        public MetaSchema Schema { get; set; }
        public IList<MetaAnnotation> Annotations { get; set; }
    }
}