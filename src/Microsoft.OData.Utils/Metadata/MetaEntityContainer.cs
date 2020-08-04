// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="EntityContainer">
    ///    <Key>
    ///      <PropertyRef Name = "QualifiedName" />
    ///    </ Key >
    ///    < Property Name="QualifiedName" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Name" Type="Edm.String" Nullable="false" />
    ///    <NavigationProperty Name = "EntitySets" Type="Collection(Meta.EntitySet)" Partner="EntityContainer" />
    ///    <NavigationProperty Name = "FunctionImports" Type="Collection(Meta.FunctionImport)" Partner="EntityContainer" />
    ///    <NavigationProperty Name = "Singletons" Type="Collection(Meta.Singleton)" Partner="EntityContainer" />
    ///    <NavigationProperty Name = "ActionImports" Type="Collection(Meta.ActionImport)" Partner="EntityContainer" />
    ///    <NavigationProperty Name = "Schema" Type="Meta.Schema" Nullable="false" Partner="EntityContainer" />
    ///    <NavigationProperty Name = "Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
    ///  </EntityType>
    /// </summary>
    public class MetaEntityContainer : MetaElement
    {
        [Key]
        public string QualifiedName { get; set; }

        public string Name { get; set; }

        public IList<MetaEntitySet> EntitySets { get; }

        public IList<MetaSingleton> Singletons { get; }

        public IList<MetaActionImport> ActionImports { get; }

        public IList<MetaFunctionImport> FunctionImports { get; }

        public MetaSchema Schema { get; set; }

        public IList<MetaAnnotation> Annotations { get; set; }
    }
}