// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Microsoft.OData.Edm;
//using Microsoft.OData.Utils.Json;
using Microsoft.OData.Utils.Meta;
using Microsoft.OData.Utils.Parser;
using Microsoft.OData.Utils.Value;
using JsonWriterOptions = System.Text.Json.JsonWriterOptions;

[assembly: InternalsVisibleToAttribute("Microsoft.OData.Utils.Tests")]

namespace Microsoft.OData.Utils
{
    public class MetaTypes
    {

    }

    public static class MetadataServiceExtensions
    {
        public static MetaCollect Types(this IEdmModel model)
        {
            return null;
        }

        public static MetaCollect Schemata(this IEdmModel model)
        {
            return null;
        }

        public static string GetMetadata(this IEdmModel model, string query)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return null;
        }

        public static IMetadata GetMetadata(this IEdmModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var meta = new Metadata(model);
            meta.Visit();
            return meta;
        }

        //public static MetaArray EntitySets(this IMetadata meta)
        //{
        //    return null;
        //}

        //public static MetaArray Types(this IMetadata meta)
        //{
        //    return null;
        //}

        /// <summary>
        /// A type is identified by its QualifiedName property,
        /// which is the Namespace of the defining schema, followed by a dot (.) and the Name of the type.
        /// There is only one entity set Types for all types.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IEnumerable<MetaType> GetTypes(this IEdmModel model)
        {
            return null;
        }

        public static IEnumerable<MetaStructuredType> GetStructuredTypes(this IEdmModel model)
        {
            return null;
        }

        public static IEnumerable<MetaEntityType> GetEntityTypes(this IEdmModel model)
        {
            return null;
        }

        public static IEnumerable<MetaComplexType> GetComplexTypes(this IEdmModel model)
        {
            return null;
        }

        public static string SerializeTypes(this IEdmModel model)
        {
            return null;
        }

        public static string Serialize<T>(this T element) where T : IMetaElement
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            IMetaSerializable serializable = element as IMetaSerializable;
            if (serializable != null)
            {
                StringBuilder sb = new StringBuilder();
                //using (StringWriter sw = new StringWriter(sb))
                //{
                //    IJsonWriter jsonWriter = new JsonWriter(sw);
                //    serializable.Serialize(jsonWriter);
                //    jsonWriter.Flush();
                //}

                return sb.ToString();
            }

            throw new InvalidOperationException(
                string.Format(CultureInfo.InvariantCulture,
                "The input element type '{0}' doesn't support serialization", element.GetType().FullName));
        }

        public static string Serialize(this IMetaValue metaValue)
        {
            if (metaValue == null)
            {
                throw new ArgumentNullException(nameof(metaValue));
            }

            var options = new JsonWriterOptions
            {
                Indented = true
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    metaValue.Serialize(writer);
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static string SerializeWithContextUri(this IMetaValue metaValue)
        {
            if (metaValue == null)
            {
                throw new ArgumentNullException(nameof(metaValue));
            }

            var options = new JsonWriterOptions
            {
                Indented = true
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    metaValue.Serialize(writer);
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }

        }
    }
}
