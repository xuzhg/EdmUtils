// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EnumType Name="FacetName" IsFlags="false" UnderlyingType="Edm.Byte">
    ///    <Member Name = "MaxLength" />
    ///    < Member Name="Unicode" />
    ///    <Member Name = "Precision" />
    ///    < Member Name="Scale" />
    ///    <Member Name = "SRID" />
    ///  </ EnumType >
    /// </summary>
    public enum MetaFacetName
    {
        MaxLength,

        Unicode,

        Precision,

        Scale,

        SRID
    }
}