using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmLib
{
    /// <summary>
    /// Represents a schema
    /// </summary>
    public class KeSchema
    {
        public string Namespace { get; set; }

        public IList<KeEntityType> EntityTypes { get; set; } = new List<KeEntityType>();

        public IList<KeComplexType> ComplexTypes { get; set; } = new List<KeComplexType>();

        public IList<KeEnumType> EnumTypes { get; set; } = new List<KeEnumType>();

        public List<IEdmSchemaElement> SchemaElements;

        public List<IEdmEntityContainer> EntityContainers;

        public Dictionary<string, List<IEdmVocabularyAnnotation>> Annotations;

        public List<string> usedNamespaces;
    }
}
