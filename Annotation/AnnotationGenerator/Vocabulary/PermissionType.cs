
using System.Collections.Generic;
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Complex type: Org.OData.Capabilities.V1.PermissionType
    /// </summary>
    internal class PermissionType : IRecord
    {
        /// <summary>
        /// Gets the auth flow scheme name.
        /// </summary>
        public string SchemeName { get; set; }

        /// <summary>
        /// Gets the list of scopes that can provide access to the resource.
        /// </summary>
        public IList<ScopeType> Scopes { get; set; }

        /// <summary>
        /// Init the <see cref="PermissionType"/>.
        /// </summary>
        /// <param name="record">The input record.</param>
        public virtual void Write(XmlWriter writer)
        {
            // Scheme
            writer.WriteStringProperty("SchemeName", SchemeName);

            // Scopes
            writer.WriteCollectionProperty("Scopes", Scopes);
        }
    }
}
