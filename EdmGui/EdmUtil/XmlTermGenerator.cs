// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.IO;
using System.Xml;
using Microsoft.OData.Edm;

namespace EdmUtil
{
    public class XmlTermGenerator : EdmTermGenerator, IDisposable
    {
        protected XmlWriter writer;

        public XmlTermGenerator(Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            writer = XmlWriter.Create(stream, settings);
        }

        protected override void WriteTermStart(string termName)
        {
            // <Annotation>
            writer.WriteStartElement("Annotation");

            writer.WriteAttributeString("Term", termName);
        }

        protected override void WriteTermEnd()
        {
            // </Annotation>
            writer.WriteEndElement();

            writer.Flush();
        }

        protected override void WriteCollectionStart()
        {
            // <Collection>
            writer.WriteStartElement("Collection");
        }

        protected override void WriteCollectionEnd()
        {
            // </Collection>
            writer.WriteEndElement();
        }

        protected override void WriteStructuredStart()
        {
            // <Record>
            writer.WriteStartElement("Record");
        }

        protected override void WriteStructuredEnd()
        {
            // </Record>
            writer.WriteEndElement();
        }

        protected override void WritePropertyStart(IEdmProperty property)
        {
            // <PropertyValue Property="" >
            writer.WriteStartElement("PropertyValue");

            writer.WriteAttributeString("Property", property.Name);
        }

        protected override void WritePropertyEnd(IEdmProperty property)
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

        public void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
            }
        }
    }
}
