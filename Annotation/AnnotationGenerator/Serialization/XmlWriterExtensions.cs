using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using Microsoft.OData.EdmUtils.Terms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace AnnotationGenerator.Serialization
{
    static class XmlWriterExtensions
    {
        /// <summary>
        /// <PropertyValue Property="PropertyName" String="PropertyValue" />
        /// </summary>
        public static void WriteStringProperty(this XmlWriter writer, string property, string value)
        {
            if (value != null)
            {
                // <PropertyValue Property="" String="" />
                writer.WriteStartElement("PropertyValue");

                writer.WriteAttributeString("Property", property);
                writer.WriteAttributeString("String", value);

                // </PropertyValue>
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// <PropertyValue Property="PropertyName" Boolean=true/false />
        /// </summary>
        public static void WriteBooleanProperty(this XmlWriter writer, string property, bool? value)
        {
            if (value != null)
            {
                // <PropertyValue Property="" String="" />
                writer.WriteStartElement("PropertyValue");

                writer.WriteAttributeString("Property", property);
                writer.WriteAttributeString("Boolean", XmlConvert.ToString(value.Value));

                // </PropertyValue>
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// <PropertyValue Property="PropertyName" >
        ///  <Collection>
        ///     <Record>
        ///       ...
        ///     </Record>
        ///     <Record>
        ///        ...
        ///     </Record>
        /// </Collection>
        /// </PropertyValue>
        /// </summary>
        public static void WriteCollectionProperty<T>(this XmlWriter writer, string property, IEnumerable<T> collections) where T : IRecord
        {
            if (collections != null && collections.Any())
            {
                // <PropertyValue>
                writer.WriteStartElement("PropertyValue");
                writer.WriteAttributeString("Property", property);

                // <Collection>
                writer.WriteStartElement("Collection");

                foreach (var item in collections)
                {
                    item.Write(writer);
                }

                // </Collection>
                writer.WriteEndElement();

                // </PropertyValue>
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// <PropertyValue Property="PropertyName" >
        ///  <Collection>
        ///    WriteEachElement
        /// </Collection>
        /// </PropertyValue>
        /// </summary>
        public static void WriteCollectionProperty<T>(this XmlWriter writer, string property, IEnumerable<T> collections,
            Action<XmlWriter, T> itemAction)
        {
            if (collections != null && collections.Any())
            {
                // <PropertyValue>
                writer.WriteStartElement("PropertyValue");
                writer.WriteAttributeString("Property", property);

                // <Collection>
                writer.WriteStartElement("Collection");

                foreach (var item in collections)
                {
                    itemAction(writer, item);
                }

                // </Collection>
                writer.WriteEndElement();

                // </PropertyValue>
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// <PropertyValue Property="PropertyName" >
        ///  <Record>
        ///    
        /// </Record>
        /// </PropertyValue>
        /// </summary>
        public static void WriteRecordProperty<T>(this XmlWriter writer, string property, T value) where T : IRecord
        {
            if (value != null)
            {
                // <PropertyValue>
                writer.WriteStartElement("PropertyValue");
                writer.WriteAttributeString("Property", property);

                // <Record>
                writer.WriteStartElement("Record");

                value.Write(writer);

                // </Record>
                writer.WriteEndElement();

                // </PropertyValue>
                writer.WriteEndElement();
            }
        }

        public static void WriteRecord<T>(this XmlWriter writer, T value) where T : IRecord
        {
            if (value != null)
            {
                // <Record>
                writer.WriteStartElement("Record");

                value.Write(writer);

                // </Record>
                writer.WriteEndElement();
            }
        }

        public static void WriteRecord<T>(this XmlWriter writer, T value, string recordType) where T : IRecord
        {
            if (value != null)
            {
                // <Record>
                writer.WriteStartElement("Record");

                // Type attribute
                writer.WriteAttributeString("Type", recordType);

                value.Write(writer);

                // </Record>
                writer.WriteEndElement();
            }
        }

        public static void WriteCollection<T>(this XmlWriter writer, IEnumerable<T> collections) where T : IRecord
        {
            if (collections != null && collections.Any())
            {
                // <Collection>
                writer.WriteStartElement("Collection");

                foreach (var item in collections)
                {
                    item.Write(writer);
                }

                // </Collection>
                writer.WriteEndElement();
            }
        }

        public static void WriteCollection<T>(this XmlWriter writer, IEnumerable<T> collections,
            Action<XmlWriter, T> itemAction)
        {
            if (collections != null && collections.Any())
            {
                // <Collection>
                writer.WriteStartElement("Collection");

                foreach (var item in collections)
                {
                    itemAction(writer, item);
                }

                // </Collection>
                writer.WriteEndElement();
            }
        }

#if false
        public static void Write(this XmlWriter writer, IRecordExpression termValue, bool typeName = false)
        {
            if (termValue == null)
            {
                return;
            }

            // <Record>
            writer.WriteStartElement("Record");

            // Type="..."
            if (typeName)
            {
                writer.WriteAttributeString("Type", termValue.FullTypeName);
            }

            Type type = termValue.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(termValue);

                IRecordExpression record = propertyValue as IRecordExpression;
                if (record != null)
                {
                    Type propertyValueType = propertyValue.GetType();
                    if (property.PropertyType != propertyValueType)
                    {
                        writer.Write(record, typeName: true);
                    }
                    else
                    {
                        writer.Write(record, typeName: false);
                    }
                }
            }

            // </Record>
            writer.WriteEndElement();
        }

        public static void WriteProperty(this XmlWriter writer, PropertyInfo propertyInfo, IRecordExpression termValue)
        {
            object propertyValue = propertyInfo.GetValue(termValue);
            if (propertyValue == null)
            {
                return;
            }
            Type propertyValueType = propertyValue.GetType();

            string propertyName = propertyInfo.Name;
            Type propertyType = propertyInfo.PropertyType;

            // <PropertyValue>
            writer.WriteStartElement("PropertyValue");
            writer.WriteAttributeString("Property", propertyName);

            IRecordExpression record = propertyValue as IRecordExpression;
            if (record != null)
            {
                if (propertyType != propertyValueType)
                {
                    writer.Write(record, typeName: true);
                }
                else
                {
                    writer.Write(record, typeName: false);
                }
            }
            else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) // Generic type
            {
                Type elementType = propertyType.GetGenericArguments()[0];
            }

            // Note that type cannot be a nullable type as value is not null and it is boxed.

            // </PropertyValue>
            writer.WriteEndElement();
        }


        public static void Write(this XmlWriter writer, TermCollectionValue termValue)
        {
            if (termValue == null)
            {
                return;
            }

            // <Collection>
            writer.WriteStartElement("Collection");

            foreach (ITermValue item in termValue.Items)
            {
                writer.Write(item);
            }

            // </Collection>
            writer.WriteEndElement();
        }

        public static void Write(this XmlWriter writer, ITermValue termValue)
        {
            if (termValue == null)
            {
                return;
            }
            

        }

        public static void Write(this XmlWriter writer, VocabularyAnnotation annotation)
        {
            if (annotation == null)
            {
                return;
            }

            // <Annotation Term="Org.OData.Cap...V1.ReadRestrictions">
            writer.WriteStartElement("Annotation");
            writer.WriteAttributeString("Term", annotation.Term.FullName());
//      writer.Write(annotation.Value);

            // </Annotation>
            writer.WriteEndElement();
        }

        public static void WriteValue(this XmlWriter writer, object value, bool isCollection = false)
        {
            if (value == null)
            {
                return;
            }

            IEnumerable enumerable = value as IEnumerable;
            if (enumerable != null)
            {
                writer.WriteCollectionValue(enumerable);
                return;
            }

            IRecord record = value as IRecord;
            //if (recrod != null)
            //{
            //    writer.WriteRecordValue(record);
            //}

        }

        public static void WriteRecordValue(this XmlWriter writer, IRecord record)
        {
            if (record == null)
            {
                return;
            }

            // <Record>
            writer.WriteStartElement("Record");

            record.Write(writer);

            // </Record>
            writer.WriteEndElement();
        }

        public static void WriteCollectionValue(this XmlWriter writer, IEnumerable value)
        {
            if (value == null)
            {
                return;
            }

            writer.WriteStartElement("Collection");

            foreach (object item in value)
            {
                writer.WriteValue(item, true);
            }

            writer.WriteEndElement();
        }

        public static void WriteStringValue(this XmlWriter writer, string value, bool isCollection = false)
        {
            if (value == null)
            {
                return;
            }

            if (isCollection)
            {
                writer.WriteStartElement("String");
                writer.WriteValue(value);
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteAttributeString("String", value);
            }
        }
    }


    public class VocabularyAnnotations
    {
        public string Target { get; set; }

        public IList<VocabularyAnnotation> Annotations { get; set; }
    }
#endif
    }
}
