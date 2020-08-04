// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    ///  <EntityType Name="TypeDefinition" BaseType="Meta.PrimitiveType">
    ///    <Property Name = "Facets" Type="Collection(Meta.Facet)" />
    ///    <NavigationProperty Name = "UnderlyingType" Type="Meta.PrimitiveType" Nullable="false" Partner="TypeDefinitions" />
    ///  </EntityType>
    /// </summary>
    public class MetaTypeDefinitionType : MetaPrimitiveType
    {
        public IList<MetaFacet> Facets { get; set; }

        public override MetaTypeKind Kind => MetaTypeKind.TypeDefinition;
    }
}