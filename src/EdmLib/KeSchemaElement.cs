// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Common base class for all named EDM elements.
    /// </summary>
    public abstract class KeSchemaElement : KeNamedElement
    {
        public KeSchemaElement(string namespaceName, string name)
            : base(name)
        {
            Namespace = namespaceName;
        }

        public KeSchemaElement(KeSchema declaringSchema, string name)
            : base(name)
        {
            DeclaringSchema = declaringSchema;
        }

        /// <summary>
        /// Gets the namespace this schema element belongs to.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Gets/sets the declared schema.
        /// </summary>
        public KeSchema DeclaringSchema { get; set; }
    }
}
