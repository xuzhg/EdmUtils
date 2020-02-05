using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Xml;

namespace VocabularyTemplate
{
    class XmlTermGenerator : TermGenerator
    {
        protected XmlWriter writer;

        public XmlTermGenerator()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            writer = XmlWriter.Create(stream, settings);
        }

        public override void WriteTermStart(string termName)
        {
            // <Annotation>
            writer.WriteStartElement("Annotation");

            writer.WriteAttributeString("Term", termName);
        }

        public override void WriteTermEnd()
        {
            // </Annotation>
            writer.WriteEndElement();

            writer.Flush();
        }

        public override void WriteCollectionStart()
        {
            // <Collection>
            writer.WriteStartElement("Collection");
        }

        public override void WriteCollectionEnd()
        {
            // </Collection>
            writer.WriteEndElement();
        }

        public override void WriteStructuredStart()
        {
            // <Record>
            writer.WriteStartElement("Record");
        }

        public override void WriteStructuredEnd()
        {
            // </Record>
            writer.WriteEndElement();
        }

        public override void WritePropertyStart(IEdmProperty property)
        {
            // <PropertyValue Property="" >
            writer.WriteStartElement("PropertyValue");

            writer.WriteAttributeString("Property", property.Name);
        }

        public override void WritePropertyEnd(IEdmProperty property)
        {
            // </PropertyValue>
            writer.WriteEndElement();
        }

        protected override void WriteStringValueTemplate(string kind, string primitiveTemplate, bool inCollection)
        {
            if (inCollection)
            {
                writer.WriteStartElement(kind);
                writer.WriteString(primitiveTemplate + "_01");
                writer.WriteEndElement();
                writer.WriteStartElement(kind);
                writer.WriteString(primitiveTemplate + "_02");
                writer.WriteEndElement();
                writer.WriteStartElement(kind);
                writer.WriteString(primitiveTemplate + "_03");
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteAttributeString(kind, primitiveTemplate);
            }
        }

        

        public override void Dispose(bool disposing)
        {
            if (writer != null)
            {
                writer.Dispose();
            }
        }
    }
}
