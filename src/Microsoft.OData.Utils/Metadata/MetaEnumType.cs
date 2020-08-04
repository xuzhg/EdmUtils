// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="EnumType" BaseType="Meta.PrimitiveType">
    ///    <Property Name = "IsFlags" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "UnderlyingType" Type="Meta.PrimitiveType" Nullable="false" Partner="EnumTypes" />
    ///    <NavigationProperty Name = "Members" Type="Collection(Meta.EnumTypeMember)" Partner="EnumType" />
    ///  </EntityType>
    /// </summary>
    public class MetaEnumType : MetaPrimitiveType
    {
        public bool IsFlag { get; set; }

        public MetaPrimitiveType UnderlyingType { get; set; }

        public List<MetaEnumTypeMember> Members { get; set; }

        public override MetaTypeKind Kind => MetaTypeKind.Enum;
    }
}