
using System;
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Complex type: Org.OData.Authorization.V1.AuthorizationScope
    /// </summary>
    internal class AuthorizationScopeType : IRecord
    {
        public string TermName => throw new NotImplementedException();

        /// <summary>
        /// Gets the Scope name.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Gets the Identity that has access to the scope or can grant access to the scope.
        /// </summary>
        public string Grant { get; set; }

        /// <summary>
        /// Gets the Description of the scope.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Init the <see cref="PermissionType"/>.
        /// </summary>
        /// <param name="record">The input record.</param>
        public virtual void Write(XmlWriter writer)
        {
            // Scope
            writer.WriteStringProperty("Scope", Scope);

            // Description
            writer.WriteStringProperty("Description", Description);

            // Grant
            writer.WriteStringProperty("Grant", Grant);
        }
    }
}
