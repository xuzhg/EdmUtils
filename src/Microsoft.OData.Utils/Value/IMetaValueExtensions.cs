// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace Microsoft.OData.Utils.Value
{
    internal static class IMetaValueExtensions
    {
        public static void Serialize(this IMetaValue metaValue, Utf8JsonWriter jsonWriter)
        {
            if (jsonWriter == null)
            {
                throw new ArgumentNullException(nameof(jsonWriter));
            }

            if (metaValue == null)
            {
                jsonWriter.WriteNullValue();
                return;
            }

            // metadata ?

            switch (metaValue.Kind)
            {
                case MetaValueKind.MObject:
                    MetaObject metaObject = (MetaObject)metaValue;
                    metaObject.Serialize(jsonWriter);
                    break;

                case MetaValueKind.MArray:
                    MetaArray metaArray = (MetaArray)metaValue;
                    metaArray.Serialize(jsonWriter);
                    break;

                case MetaValueKind.MString:
                    MetaString metaString = (MetaString)metaValue;
                    jsonWriter.WriteStringValue(metaString.Value);
                    break;

                case MetaValueKind.MBoolean:
                    MetaBoolean metaBoolean = (MetaBoolean)metaValue;
                    jsonWriter.WriteBooleanValue(metaBoolean.Value);
                    break;

                case MetaValueKind.MInt32:
                    MetaInt32 metaInt32 = (MetaInt32)metaValue;
                    jsonWriter.WriteNumberValue(metaInt32.Value);
                    break;
            }
        }

        private static void Serialize(this MetaObject metaObject, Utf8JsonWriter jsonWriter)
        {
            Debug.Assert(metaObject != null);
            Debug.Assert(jsonWriter != null);

            jsonWriter.WriteStartObject();

            foreach (var item in metaObject)
            {
                jsonWriter.WritePropertyName(item.Key);

                item.Value.Serialize(jsonWriter);
            }

            jsonWriter.WriteEndObject();
        }

        private static void Serialize(this MetaArray metaArray, Utf8JsonWriter jsonWriter)
        {
            Debug.Assert(metaArray != null);
            Debug.Assert(jsonWriter != null);

            jsonWriter.WriteStartArray();

            foreach (var item in metaArray)
            {
                item.Serialize(jsonWriter);
            }

            jsonWriter.WriteEndArray();
        }

        //public static T Select<T>(this T metaArray, Func<T, bool> selector)
        //    where T : IMetaValue
        //{
        //    foreach (var item in metaArray)
        //    {

        //    }
        //}

        // $top=n
        public static MetaArray Top(this MetaArray array, int top)
        {
            MetaArray newArray = new MetaArray();
            foreach (var item in array.Take(top))
            {
                newArray.Add(item);
            }

            return newArray;
        }

        // $skip=n
        //public static MetaArray Skip(this MetaArray array, int skip)
        //{
        //    MetaArray newArray = new MetaArray();
        //    foreach (var item in array.Skip(skip))
        //    {
        //        newArray.Add(item);
        //    }

        //    return newArray;
        //}

        //public static MetaArray Skip(this MetaArray array, int skip)
        //{
        //    MetaArray newArray = new MetaArray();
        //    foreach (var item in array.Skip(skip))
        //    {
        //        newArray.Add(item);
        //    }

        //    return newArray;
        //}
    }
}