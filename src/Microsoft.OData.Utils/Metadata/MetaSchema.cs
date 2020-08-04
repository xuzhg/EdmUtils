// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Utils.Value;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /*
     * <EntityType Name="Schema">
        <Key>
          <PropertyRef Name="Namespace" />
        </Key>
        <Property Name="Namespace" Type="Edm.String" Nullable="false" />
        <Property Name="Alias" Type="Edm.String" />
        <NavigationProperty Name="Reference" Type="Meta.Reference" />
        <NavigationProperty Name="Types" Type="Collection(Meta.Type)" Partner="Schema" />
        <NavigationProperty Name="Actions" Type="Collection(Meta.Action)" Partner="Schema" />
        <NavigationProperty Name="Functions" Type="Collection(Meta.Function)" Partner="Schema" />
        <NavigationProperty Name="EntityContainer" Type="Meta.EntityContainer" Nullable="false" Partner="Schema" />
        <NavigationProperty Name="Terms" Type="Collection(Meta.Term)" Partner="Schema" />
        <NavigationProperty Name="Annotations" Type="Collection(Meta.Annotation)" Partner="Target" />
      </EntityType>
    */
    public class MetaSchema : MetaElement
    {
        public string Namespace { get; }

        public string Alias { get; }

        public MetaReference Reference { get; set; }

        public IList<MetaType> Types { get; }

        public IList<MetaAction> Actions { get; }

        public IList<MetaFunction> Functions { get; }

        public IList<MetaTerm> Terms { get; }

        public MetaEntityContainer EntityContainer { get; }

        public IList<MetaAnnotation> Annotations { get; set; }
    }
}