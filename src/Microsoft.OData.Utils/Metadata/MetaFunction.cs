// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="Function">
    ///    <Key>
    ///      <PropertyRef Name = "QualifiedName" />
    ///    </ Key >
    ///    < Property Name="QualifiedName" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Overloads" Type="Collection(Meta.FunctionOverload)" />
    ///    <NavigationProperty Name = "FunctionImports" Type="Collection(Meta.FunctionImport)" Partner="Function" />
    ///    <NavigationProperty Name = "Schema" Type="Meta.Schema" Nullable="false" Partner="Functions" />
    ///  </EntityType>
    /// </summary>
    public class MetaFunction : MetaOperation
    {
        [Key]
        public string QualifiedName { get; set; }

        public string Name { get; set; }

        public IList<MetaFunctionOverload> Overloads { get; set; }

        public IList<MetaFunctionImport> FunctionImports { get; set; }

        public MetaSchema Schema { get; set; }
    }
}