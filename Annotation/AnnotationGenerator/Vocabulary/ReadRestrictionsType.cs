
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.ReadRestrictionsBase
    /// </summary>
    internal abstract class ReadRestrictionsBase : PermissionsRecord, IRecord
    {
        /// <summary>
        /// Get the Entities can be retrieved.
        /// </summary>
        public bool? Readable { get; private set; }

        public virtual void Write(XmlWriter writer)
        {
            // Readable
            writer.WriteBooleanProperty("Readable", Readable);

            // Permissions
            writer.WriteCollectionProperty("Permissions", Permissions);
        }
    }

    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.ReadByKeyRestrictionsType
    /// Restrictions for retrieving an entity by key
    /// </summary>
    internal class ReadByKeyRestrictions : ReadRestrictionsBase
    {
        // nothing here
    }

    /// <summary>
    /// Complex Type: Org.OData.Capabilities.V1.ReadRestrictionsType
    /// </summary>
    //[Term("Org.OData.Capabilities.V1.ReadRestrictions")]
    internal class ReadRestrictionsType : ReadRestrictionsBase
    {
        /// <summary>
        /// Gets the Restrictions for retrieving an entity by key
        /// </summary>
        public ReadByKeyRestrictions ReadByKeyRestrictions { get; set; }

        public override void Write(XmlWriter writer)
        {
            base.Write(writer);

            // ReadByKeyRestrictions
            writer.WriteRecordProperty("ReadByKeyRestrictions", ReadByKeyRestrictions);
        }
    }
}
