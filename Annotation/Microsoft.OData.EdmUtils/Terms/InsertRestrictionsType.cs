// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;
using System.Xml;

namespace Microsoft.OData.EdmUtils.Terms
{
    /// <summary>
    /// Complex type: Org.OData.Capabilities.V1.ScopeType
    /// </summary>
    public class ScopeType : IRecord
    {
        /// <summary>
        /// Gets the names of the scope.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Gets the restricted properties.
        /// Comma-separated string value of all properties that will be included or excluded when using the scope.
        /// Possible string value identifiers when specifying properties are '*', _PropertyName_, '-'_PropertyName_.
        /// </summary>
        public string RestrictedProperties { get; set; }

        public string FullTypeName => throw new System.NotImplementedException();

        public void Write(XmlWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Complex Type: Complex type: Org.OData.Capabilities.V1.PermissionType
    /// </summary>
    public class PermissionType : IRecord
    {
        /// <summary>
        /// Gets the auth flow scheme name.
        /// </summary>
        public string SchemeName { get; set; }

        /// <summary>
        /// Gets the list of scopes that can provide access to the resource.
        /// </summary>
        public IList<ScopeType> Scopes { get; set; }

        public string FullTypeName => throw new System.NotImplementedException();

        public void Write(XmlWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Complex Type: Complex type: Org.OData.Capabilities.V1.PermissionType
    /// </summary>
    public class ModificationQueryOptionsType : IRecord
    {
        /// <summary>
        /// Gets the auth flow scheme name.
        /// </summary>
        public string SchemeName { get; set; }

        /// <summary>
        /// Gets the list of scopes that can provide access to the resource.
        /// </summary>
        public IList<ScopeType> Scopes { get; set; }

        public string FullTypeName => throw new System.NotImplementedException();

        public void Write(XmlWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Complex Type: Complex type: Org.OData.Capabilities.V1.PermissionType
    /// </summary>
    public class CustomParameter : IRecord
    {
        /// <summary>
        /// Gets the auth flow scheme name.
        /// </summary>
        public string SchemeName { get; set; }

        /// <summary>
        /// Gets the list of scopes that can provide access to the resource.
        /// </summary>
        public IList<ScopeType> Scopes { get; set; }

        public string FullTypeName => throw new System.NotImplementedException();

        public void Write(XmlWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class InsertRestrictionsType : IRecord
    {
        public bool? Insertable { get; }

        public IList<EdmPropertyPath> NonInsertableProperties { get; }

        public IList<EdmNavigationPropertyPath> NonInsertableNavigationProperties { get; }

        public int? MaxLevels { get; set; }

        public bool? TypecastSegmentSupported { get; set; }

        public IList<PermissionType> Permissions { get; set; }

        public IList<ModificationQueryOptionsType> QueryOptions { get; set; }

        public IList<CustomParameter> CustomHeaders { get; set; }

        public IList<CustomParameter> CustomQueryOptions { get; set; }
        
        public string Description { get; set; }

        public string LongDescription { get; set; }

        public string FullTypeName => "Org.OData.Capabilities.V1.InsertRestrictionsType";

        public void Write(XmlWriter writer)
        {/*
            // Readable
            writer.WriteBooleanProperty("Insertable", Insertable);

            // Permissions
            writer.WriteCollectionProperty("Permissions", Permissions, (w, t) => w.WriteRecord(t));

            // Description
            writer.WriteStringProperty("Description", Description);

            // LongDescription
            writer.WriteStringProperty("LongDescription", LongDescription);*/
        }
    }
}
