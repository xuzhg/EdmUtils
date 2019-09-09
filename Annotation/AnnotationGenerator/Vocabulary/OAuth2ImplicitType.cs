// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Xml;
using AnnotationGenerator.Serialization;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Complex Type: Complex type: Org.OData.Authorization.V1.OAuth2Implicit
    /// </summary>
    internal class OAuth2ImplicitType : OAuthAuthorizationType
    {
        /// <summary>
        /// Gets the Authorization URL.
        /// </summary>
        public string AuthorizationUrl { get; set; }

        public override string TermName => throw new System.NotImplementedException();

        /// <summary>
        /// Init the <see cref="PermissionType"/>.
        /// </summary>
        /// <param name="record">The input record.</param>
        public override void Write(XmlWriter writer)
        {
            base.Write(writer);

            // AuthorizationUrl
            writer.WriteStringProperty("AuthorizationUrl", AuthorizationUrl);
        }
    }
}
