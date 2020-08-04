// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <ComplexType Name="FunctionOverload">
    ///    <Property Name = "IsBound" Type="Edm.Boolean" Nullable="false" />
    ///    <Property Name = "IsComposable" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "ReturnType" Type="Meta.ReturnType" Nullable="false" />
    ///    <Property Name = "EntitySetPath" Type="Edm.String" />
    ///    <Property Name = "Parameters" Type="Collection(Meta.Parameter)" />
    ///    <Property Name = "Annotations" Type="Collection(Meta.InlineAnnotation)" />
    ///  </ComplexType>
    /// </summary>
    public class MetaFunctionOverload
    {
        public bool IsBound { get; set; }

        public bool IsComposable { get; set; }

        public MetaReturnType ReturnType { get; set; }

        public string EntitySetPath { get; set; }

        public IList<MetaParameter> Parameters { get; set; }

        public IList<MetaInlineAnnotation> Annotations { get; set; }
    }
}