// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="StructuredType" BaseType="Meta.Type" Abstract="true">
    ///    <Property Name = "Abstract" Type="Edm.Boolean" Nullable="false" />
    ///    <Property Name = "OpenType" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "Properties" Type="Collection(Meta.Property)" Partner="DeclaringType" />
    ///    <NavigationProperty Name = "NavigationProperties" Type="Collection(Meta.NavigationProperty)" Partner="DeclaringType" />
    ///  </EntityType>
    /// </summary>
    public abstract class MetaStructuredType : MetaType
    {
        public bool Abstract { get; set; }

        public bool OpenType { get; set; }

        public IList<MetaProperty> Properties { get; set; }
        public IList<MetaNavigationProperty> NavigationProperties { get; set; }
    }
}