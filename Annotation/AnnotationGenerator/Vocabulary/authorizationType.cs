
using System;
using System.Collections.Generic;
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Complex type: Org.OData.Authorization.V1.Authorization
    /// </summary>
    internal abstract class AuthorizationType : IRecord
    {
        public abstract string TermName { get; }

        /// <summary>
        /// Gets the Name that can be used to reference the authorization scheme.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the Description of the authorization scheme.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Init the <see cref="PermissionType"/>.
        /// </summary>
        /// <param name="record">The input record.</param>
        public virtual void Write(XmlWriter writer)
        {
            // Name
            writer.WriteStringProperty("Name", Name);

            // Description
            writer.WriteStringProperty("Description", Description);
        }
    }
}
