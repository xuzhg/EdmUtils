
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.OperationRestrictionsType
    /// </summary>
    internal class OperationRestrictionsType : PermissionsRecord, IRecord
    {
        public string TermName => "Org.OData.Capabilities.V1.OperationRestrictions";

        public virtual void Write(XmlWriter writer)
        {
            // Permissions
            writer.WriteCollectionProperty("Permissions", Permissions, (w, t) => w.WriteRecord(t));
        }
    }
}
