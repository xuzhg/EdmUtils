
using System.Collections.Generic;
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.DeleteRestrictionsType
    /// </summary>
    internal class DeleteRestrictionsType : PermissionsRecord, IRecord
    {
        public string TermName => "Org.OData.Capabilities.V1.DeleteRestrictions";

        /// <summary>
        /// Gets the Deletable value.
        /// </summary>
        public bool? Deletable { get; private set; }

        /// <summary>
        /// Gets the navigation properties which do not allow DeleteLink requests.
        /// </summary>
        public IList<string> NonDeletableNavigationProperties { get; private set; }

        public string Description { get; set; }
        public string LongDescription { get; set; }

        public virtual void Write(XmlWriter writer)
        {
            // Readable
            writer.WriteBooleanProperty("Deletable", Deletable);

            // NonDeletableNavigationProperties
            writer.WriteCollectionProperty("NonDeletableNavigationProperties",
                NonDeletableNavigationProperties,
                (w, item) =>
                {
                    w.WriteStartElement("String");
                    w.WriteValue(item);
                    w.WriteEndElement();
                });

            // Permissions
            writer.WriteCollectionProperty("Permissions", Permissions);

            // Description
            writer.WriteStringProperty("Description", Description);

            // LongDescription
            writer.WriteStringProperty("LongDescription", LongDescription);
        }
    }
}
