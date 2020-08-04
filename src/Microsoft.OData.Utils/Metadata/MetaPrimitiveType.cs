// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Utils.Json;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="PrimitiveType" BaseType="Meta.Type">
    ///    <NavigationProperty Name = "TypeDefinitions" Type="Collection(Meta.TypeDefinition)" Partner="UnderlyingType" />
    ///    <NavigationProperty Name = "EnumTypes" Type="Collection(Meta.EnumType)" Partner="UnderlyingType" />
    ///  </EntityType>
    /// </summary>
    public class MetaPrimitiveType : MetaType
    {
        public IList<MetaTypeDefinitionType> TypeDefinitions { get; set; }

        public IList<MetaEnumType> EnumTypes { get; set; }

        public override MetaTypeKind Kind => MetaTypeKind.Primitive;

    }
}