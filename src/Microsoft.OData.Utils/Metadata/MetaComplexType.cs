// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Utils.Json;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="ComplexType" BaseType="Meta.StructuredType">
    ///    <NavigationProperty Name = "BaseType" Type="Meta.ComplexType" Partner="DerivedTypes" />
    ///    <NavigationProperty Name = "DerivedTypes" Type="Collection(Meta.ComplexType)" Partner="BaseType" />
    ///  </EntityType>
    /// </summary>
    public class MetaComplexType : MetaStructuredType
    {
        public override MetaTypeKind Kind => MetaTypeKind.Complex;

        public MetaComplexType BaseType { get; set; }

        public IList<MetaComplexType> DerivedTypes { get; set; }
    }
}