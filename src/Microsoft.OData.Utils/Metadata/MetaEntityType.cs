// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Utils.Json;
using System;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    /// <summary>
    /// <EntityType Name="EntityType" BaseType="Meta.StructuredType">
    ///    <Property Name = "Key" Type="Collection(Meta.KeyProperty)" />
    ///    <Property Name = "HasStream" Type="Edm.Boolean" Nullable="false" />
    ///    <NavigationProperty Name = "BaseType" Type="Meta.EntityType" Partner="DerivedTypes" />
    ///    <NavigationProperty Name = "DerivedTypes" Type="Collection(Meta.EntityType)" Partner="BaseType" />
    ///    <NavigationProperty Name = "EntitySets" Type="Collection(Meta.EntitySet)" Partner="EntityType" />
    ///  </EntityType>
    /// </summary>
    public partial class MetaEntityType : MetaStructuredType
    {
        public bool HasStream { get; set; }

        public IList<MetaKeyProperty> Key { get; }

        public MetaEntityType BaseType { get; }

        public IList<MetaEntityType> DerivedTypes { get; }

        public IList<MetaEntitySet> EntitySets { get; }

        public override MetaTypeKind Kind => MetaTypeKind.Entity;
    }

    //public partial class MetaEntityType : IMetaSerializable
    //{
    //    public void Serialize(IJsonWriter jsonWriter)
    //    {
    //        if (jsonWriter == null)
    //        {
    //            throw new ArgumentNullException(nameof(jsonWriter));
    //        }


    //        jsonWriter.StartObjectScope();
    //        jsonWriter.WriteRequiredProperty("@odata.type", "Meta.EntityType");
    //        jsonWriter.WriteRequiredProperty("QualifiedName", Namespace + "." + Name);
    //        jsonWriter.WriteRequiredProperty("Name", Name);

    //        // Key collection


    //        jsonWriter.WriteRequiredProperty("Abstract", IsAbstract);

    //        jsonWriter.WriteRequiredProperty("OpenType", IsOpen);
    //        jsonWriter.WriteRequiredProperty("HasStream", HasStream);

    //        jsonWriter.EndObjectScope();
    //    }
    //}
}