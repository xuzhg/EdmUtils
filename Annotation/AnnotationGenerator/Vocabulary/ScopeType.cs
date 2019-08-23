
using System;
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex type: Org.OData.Capabilities.V1.ScopeType
    /// </summary>
    internal class ScopeType : IRecord
    {
        public string TermName => throw new NotImplementedException();

        /// <summary>
        /// Gets the names of the scope.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Gets the restricted properties.
        /// Comma-separated string value of all properties that will be included or excluded when using the scope.
        /// Possible string value identifiers when specifying properties are '*', _PropertyName_, '-'_PropertyName_.
        /// </summary>
        public string RestrictedProperties { get; set; }

        /// <summary>
        /// Init the <see cref="ScopeType"/>.
        /// </summary>
        /// <param name="record">The input record.</param>
        public virtual void Write(XmlWriter writer)
        {
            // Scope
            writer.WriteStringProperty("Scope", Scope);

            // RestrictedProperties
            writer.WriteStringProperty("RestrictedProperties", RestrictedProperties);
        }
    }
}
