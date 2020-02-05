using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.IO;
using VocabularyTemplate.Writer;

namespace VocabularyTemplate
{
    class JsonTermGenerator : JsonYamlTermGeneratorBase
    {
        protected JsonWriter jsonWriter;

        public JsonTermGenerator()
        {
            jsonWriter = new JsonWriter(new StreamWriter(stream));
            writer = jsonWriter;
        }
    }
}
