
using System.Collections.Generic;
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.UpdateRestrictionsType
    /// </summary>
    internal class UpdateRestrictionsType : PermissionsRecord, IRecord
    {
        public string TermName => "Org.OData.Capabilities.V1.UpdateRestrictions";

        /// <summary>
        /// Gets the Updatable value, if true, entities can be updated.
        /// The default value is true;
        /// </summary>
        public bool? Updatable { get; private set; }

        /// <summary>
        /// Gets the navigation properties which do not allow rebinding.
        /// </summary>
        public IList<string> NonUpdatableNavigationProperties { get; private set; }

        public string Description { get; set; }
        public string LongDescription { get; set; }

        public virtual void Write(XmlWriter writer)
        {
            // Readable
            writer.WriteBooleanProperty("Updatable", Updatable);

            // NonUpdatableNavigationProperties
            writer.WriteCollectionProperty("NonUpdatableNavigationProperties",
                NonUpdatableNavigationProperties,
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
