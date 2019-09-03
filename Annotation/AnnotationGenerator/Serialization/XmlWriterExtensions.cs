using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnnotationGenerator.Serialization
{
    static class XmlWriterExtensions
    {
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
    }
}
