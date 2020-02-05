using System.IO;
using VocabularyTemplate.Writer;

namespace VocabularyTemplate
{
    class YamlTermGenerator : JsonYamlTermGeneratorBase
    {
        protected YamlWriter yamlWriter;

        public YamlTermGenerator()
        {
            yamlWriter = new YamlWriter(new StreamWriter(stream));
            writer = yamlWriter;
        }


    }
}
