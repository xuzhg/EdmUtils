using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.IO;

namespace VocabularyTemplate
{
    abstract class JsonYamlTermGeneratorBase : TermGenerator
    {
        protected ITermWriter writer;

        public override void WriteTermStart(string termName)
        {
            writer.StartObjectScope();

            writer.WritePropertyName("@" + termName);
        }

        public override void WriteTermEnd()
        {
            writer.EndObjectScope();
            writer.Flush();
        }

        public override void WriteCollectionStart()
        {
            writer.StartArrayScope();
        }

        public override void WriteCollectionEnd()
        {
            writer.EndArrayScope();
        }

        public override void WriteStructuredStart()
        {
            writer.StartObjectScope();
        }

        public override void WriteStructuredEnd()
        {
            writer.EndObjectScope();
        }

        public override void WritePropertyStart(IEdmProperty property)
        {
            // <PropertyValue Property="" >
            writer.WritePropertyName(property.Name);
        }


        public override void WritePropertyEnd(IEdmProperty property)
        {
        }

        protected override void WriteStringValueTemplate(string kind, string primitiveTemplate, bool inCollection)
        {
            if (inCollection)
            {
                writer.WriteValue(primitiveTemplate + "_01");
                writer.WriteValue(primitiveTemplate + "_02");
                writer.WriteValue(primitiveTemplate + "_03");
            }
            else
            {
                writer.WriteValue(primitiveTemplate);
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
