using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using VocabularyTemplate.Writer;

namespace VocabularyTemplate
{
    public class GeneartorFactor
    {
        public static TermGenerator Create(string format)
        {
            switch (format)
            {
                case "yaml":
                    return new YamlTermGenerator();

                case "json":
                    return new JsonTermGenerator();

                default:
                    return new XmlTermGenerator();
            }
        }
    }
}
