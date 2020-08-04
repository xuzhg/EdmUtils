// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <ComplexType Name="OnDelete">
    ///    <Property Name = "Action" Type="Meta.OnDeleteAction" Nullable="false" />
    ///    <Property Name = "Annotations" Type="Collection(Meta.InlineAnnotation)" />
    ///  </ComplexType>
    /// </summary>
    public class MetaOnDelete : MetaElement
    {
        public MetaOnDeleteAction Action { get; set; }

        public IList<MetaInlineAnnotation> Annotations { get; set; }
    }


    /// <summary>
    /// <EnumType Name="OnDeleteAction">
    ///    <Member Name = "Cascade" />
    ///    < Member Name="None" />
    ///    <Member Name = "SetDefault" />
    ///    < Member Name="SetNull" />
    ///  </EnumType>
    /// </summary>
    public enum MetaOnDeleteAction
    {
        Cascade,

        None,

        SetDefault,

        SetNull
    }
}