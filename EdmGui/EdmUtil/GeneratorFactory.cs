// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System.IO;

namespace EdmUtil
{
    public static class GeneratorFactory
    {
        public static string GenerateTermTemplate(IEdmModel model, IEdmTerm term, TemplateFormat format)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                EdmTermGenerator generator;
                switch (format)
                {
                    case TemplateFormat.Yaml:
                        generator = new YamlTermGenerator(stream);
                        break;

                    case TemplateFormat.Json:
                        generator = new JsonTermGenerator(stream);
                        break;

                    case TemplateFormat.Xml:
                    default:
                        generator = new XmlTermGenerator(stream);
                        break;
                }

                generator.Create(model, term);

                stream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream);

                // Removing xml header to make the baseline's more compact and focused on the test at hand.
                return reader.ReadToEnd();
            }
        }
    }
}
