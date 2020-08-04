// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    public enum MetaTypeKind
    {
        Primitive,
        Entity,
        Complex,
        Enum,
        TypeDefinition,
    }

    /// <summary>
    /// <EntityType Name="Type" Abstract="true">
    ///  <Key>
    ///      <PropertyRef Name = "QualifiedName" />
    ///    </ Key >
    ///    < Property Name="QualifiedName" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <NavigationProperty Name = "Schema" Type="Meta.Schema" Nullable="false" Partner="Types" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>
    public abstract class MetaType : MetaElement
    {
        [Key]
        public string QualifiedName { get; set; }

        public string Namespace { get; set; }

        public string Name { get; set; }

        public MetaSchema Schema { get; set; }

        public IList<MetaAnnotation> Annotations { get; set; }

        public abstract MetaTypeKind Kind { get; }
    }
}