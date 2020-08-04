// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="Action">
    ///    <Key>
    ///      <PropertyRef Name = "QualifiedName" />
    ///    </ Key >
    ///    < Property Name="QualifiedName" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Overloads" Type="Collection(Meta.ActionOverload)" />
    ///    <NavigationProperty Name = "ActionImports" Type="Collection(Meta.ActionImport)" Partner="Action" />
    ///    <NavigationProperty Name = "Schema" Type="Meta.Schema" Nullable="false" Partner="Actions" />
    ///  </EntityType>
    /// </summary>
    public class MetaAction : MetaOperation
    {
        [Key]
        public string QualifiedName { get; set; }

        public string Name { get; set; }

        public IList<MetaActionOverload> Overloads { get; set; }

        public IList<MetaActionImport> ActionImports { get; set; }

        public MetaSchema Schema { get; set; }
    }
}