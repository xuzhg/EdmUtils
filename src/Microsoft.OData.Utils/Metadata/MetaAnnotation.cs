// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="Annotation">
    ///    <Key>
    ///      <PropertyRef Name = "Fullname" />
    ///    </ Key >
    ///    < Property Name="Fullname" Type="Edm.String" Nullable="false" />
    ///    <Property Name = "Qualifier" Type="Edm.String" />
    ///    <Property Name = "Value" Type="Meta.AnnotationExpression" Nullable="false" />
    ///    <Property Name = "Annotations" Type="Collection(Meta.InlineAnnotation)" />
    ///    <NavigationProperty Name = "Term" Type="Meta.Term" Nullable="false" Partner="Applications" />
    ///    <NavigationProperty Name = "Target" Type="Edm.EntityType" Nullable="false" Partner="Annotations" />
    ///  </EntityType>
    /// </summary>
    public class MetaAnnotation : MetaElement
    {
        [Key]
        public string Fullname { get; set; }
        public string Qualifier { get; set; }

        public MetaAnnotationExpression Value { get; }

        public IList<MetaAnnotation> Annotations { get; }
        public MetaTerm Term { get; }

        public IList<MetaEntityType> Target { get; }
    }
}