
using System.Collections.Generic;
using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Complex type: Org.OData.Authorization.V1.OAuthAuthorization
    /// </summary>
    internal abstract class OAuthAuthorizationType : AuthorizationType
    {
        /// <summary>
        /// Gets the Name that can be used to reference the authorization scheme.
        /// </summary>
        public IList<AuthorizationScopeType> Scopes { get; set; }

        /// <summary>
        /// Gets the Refresh Url.
        /// </summary>
        public string RefreshUrl { get; set; }

        /// <summary>
        /// Init the <see cref="PermissionType"/>.
        /// </summary>
        /// <param name="record">The input record.</param>
        public override void Write(XmlWriter writer)
        {
            base.Write(writer);

            // Scopes
            writer.WriteCollectionProperty("Scopes", Scopes, (x, a) => x.WriteRecord(a));

            // Scheme
            writer.WriteStringProperty("RefreshUrl", RefreshUrl);
        }
    }
}
