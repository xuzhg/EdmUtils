// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <ComplexType Name="KeyProperty">
    ///    <Property Name = "PropertyPath" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Alias" Type="Edm.String" />
    ///    <NavigationProperty Name = "Property" Type="Meta.Property" Nullable="false" />
    ///  </ComplexType>
    /// </summary>
    public class MetaKeyProperty : MetaElement
    {
        public string PropertyPath { get; }

        public string Alias { get; }

        public MetaProperty Property { get; set; }
    }
}