using AnnotationGenerator.Serialization;
using AnnotationGenerator.Vocabulary;
using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace AnnotationGenerator
{
    class AnnotationGenerator
    {
        protected MemoryStream stream;
        protected XmlWriter writer;
        protected IEdmModel model;

        public AnnotationGenerator(IEdmModel model)
        {
            stream = new MemoryStream();
            this.model = model;

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            writer = XmlWriter.Create(stream, settings);
        }

        public void Generator(ReadRestrictionsType read)
        {

        }

        public void Write(ITerm term)
        {

            if (!term.IsInLine)
            {
                // </Annotation>
                writer.WriteStartElement("Annotations");
                writer.WriteAttributeString("Target", term.Target);
            }

            // <Annotation Term="Org.OData.Cap...V1.ReadRestrictions">
            writer.WriteStartElement("Annotation");

            writer.WriteAttributeString("Term", "Org.OData.Capabilities.V1.ReadRestrictions");

            if (term.IsCollection)
            {
                writer.WriteCollection(term.Records);
            }
            else
            {
                writer.WriteRecord(term.Records.First());
            }

            // </Annotation>
            writer.WriteEndElement();

            if (!term.IsInLine)
            {
                // </Annotations>
                writer.WriteEndElement();
            }
        }

        public void SaveAs(string fileName)
        {
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(stream);

            // Removing xml header to make the baseline's more compact and focused on the test at hand.
            string result = reader.ReadToEnd();

            File.WriteAllText(fileName, result);

            stream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            writer = XmlWriter.Create(stream, settings);
        }
    }

    public interface ITerm
    {
        string Target { get; set; }

        bool IsInLine { get; set; }

        string TermName { get; }

        bool IsCollection { get; }

        IList<IRecord> Records { get; set; }
    }

    public class ReadRestrictions : ITerm
    {
        public string TermName => "Org.OData.Capabilities.V1.ReadRestrictions";

        public string Target { get; set; }

        public bool IsInLine { get; set; }

        public bool IsCollection => false;

        public IList<IRecord> Records { get; set; } = new List<IRecord>();
    }
}
