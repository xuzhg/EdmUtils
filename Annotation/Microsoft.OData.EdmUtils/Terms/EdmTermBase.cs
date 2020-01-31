// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.OData.EdmUtils.Terms
{
    
    public interface IEdmTermXmlSerializable
    {
        void Write(XmlWriter writer);
    }

    public abstract class EdmTermBase : IEdmTermXmlSerializable
    {
        public abstract string Namespace { get; }

        public abstract string Alias { get; }

        public abstract string Name { get; }

        public abstract EdmTermApplyToKind AppliesTo  { get; }

        public abstract void Write(XmlWriter writer);
    }

    public abstract class CapabilitiesVocabulariesTermBase : EdmTermBase
    {
        public override string Namespace => "Org.OData.Capabilities.V1";

        public override string Alias => "Capabilities";
    }

    public class FilterRestrictionsTerm : CapabilitiesVocabulariesTermBase, IEdmRecordType
    {
        public override string Name => "FilterRestrictions";

        public override EdmTermApplyToKind AppliesTo => EdmTermApplyToKind.EntitySet;

        public FilterRestrictionsType Restriction { get; }

        public override void Write(XmlWriter writer)
        {

        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DefaultValueAttribute : Attribute
    {
        public DefaultValueAttribute(int intValue)
        {
            DefaultInt32Value = intValue;
        }

        public DefaultValueAttribute(bool booleanValue)
        {
            DefaultBooleanValue = booleanValue;
        }

        public int? DefaultInt32Value { get; }

        public bool? DefaultBooleanValue { get; }

        public bool Nullable { get; set; }

        public string Type { get; set; }
    }


    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EdmPropertyAttributesAttribute : Attribute
    {

        public EdmPropertyAttributesAttribute()
            : this(null)
        {
        }

        public EdmPropertyAttributesAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public object DefaultValue { get; }

        public string Type { get; set; }

        public bool Nullable { get; set; } = true;

    }

    public class EdmPath
    {
        public EdmPath(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }

    public class EdmPropertyPath : EdmPath
    {
        public EdmPropertyPath(string path)
            : base(path)
        {
        }
    }

    public class EdmNavigationPropertyPath : EdmPath
    {
        public EdmNavigationPropertyPath(string path)
            : base(path)
        {
        }
    }

    public class FilterRestrictionsType : TermRecordValue
    {
        [DefaultValue(true)]
        public bool? Filterable { get; }

        [DefaultValue(false, Nullable = true)]
        public bool? RequiresFilter { get; }

        [EdmPropertyAttributes(Type = "Edm.Int32", Nullable = false)]
        public IList<EdmPropertyPath> RequiredProperties { get; set; }

        [EdmPropertyAttributes(Type = "Edm.Int32", Nullable = false)]
        public IList<EdmPropertyPath> NonFilterableProperties { get; set; }

        [EdmPropertyAttributes(Type = "Edm.Int32", Nullable = false)]
        public IList<FilterExpressionRestrictionType> FilterExpressionRestrictions { get; set; }

        [EdmPropertyAttributes(-1, Type = "Edm.Int32", Nullable = false)]
        public int MaxLevels { get; }

        public override void WriteObject(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public class FilterExpressionRestrictionType
    {
        [EdmPropertyAttributes]
        public EdmPropertyPath Property { get; set; }

        [EdmPropertyAttributes]
        public string AllowedExpressions { get; set; }
    }
}
