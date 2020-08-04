// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// EntityType Name="Reference">
    ///    <Key>
    ///      <PropertyRef Name = "Uri" />
    ///    </ Key >
    ///    < Property Name="Uri" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Include" Type="Collection(Meta.Include)" />
    ///    <Property Name = "IncludeAnnotations" Type="Collection(Meta.IncludeAnnotations)" />
    ///    <Property Name = "Annotations" Type="Collection(Meta.InlineAnnotation)" />
    ///  </EntityType>
    /// </summary>
    public class MetaReference : MetaElement
    {
        [Key]
        public string Uri { get; set; }

        public IList<MetaInclude> Include { get; set; }

        public IList<MetaIncludeAnnotation> IncludeAnnotations { get; set; }

        public IList<MetaInlineAnnotation> Annotations { get; set; }
    }

    /// <summary>
    ///  <ComplexType Name="Include">
    //    <Property Name = "Alias" Type="Edm.String" />
    //    <NavigationProperty Name = "Schema" Type="Meta.Schema" Nullable="false" />
    //  </ComplexType>
    /// </summary>
    public class MetaInclude : MetaElement
    {
        public string Alias { get; set; }

        public MetaSchema Schema { get; set; }
    }

    /// <summary>
    /// <ComplexType Name="IncludeAnnotations">
    ///    <Property Name = "TargetNamespace" Type="Edm.String" />
    ///    <Property Name = "TermNamespace" Type="Edm.String" />
    ///    <Property Name = "Qualifier" Type="Edm.String" />
    /// </ComplexType>
    /// </summary>
    public class MetaIncludeAnnotation : MetaElement
    {
        public string TargetNamespace { get; set; }

        public string TermNamespace { get; set; }

        public string Qualifier { get; set; }
    }
}