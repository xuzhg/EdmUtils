
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.InsertRestrictionsType
    /// </summary>
    internal class InsertRestrictionsType : PermissionsRecord, IRecord
    {
        public string TermName => "Org.OData.Capabilities.V1.InsertRestrictions";

        /// <summary>
        /// Gets the Insertable value.
        /// </summary>
        public bool? Insertable { get; private set; }

        public string Description { get; set; }

        public string LongDescription { get; set; }

        public virtual void Write(XmlWriter writer)
        {
            // Readable
            writer.WriteBooleanProperty("Insertable", Insertable);

            // Permissions
            writer.WriteCollectionProperty("Permissions", Permissions, (w, t) => w.WriteRecord(t));

            // Description
            writer.WriteStringProperty("Description", Description);

            // LongDescription
            writer.WriteStringProperty("LongDescription", LongDescription);
        }
    }
}
