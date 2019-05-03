// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the "Edm.Type".
    /// </summary>
    public class KeTerm : KeSchemaElement
    {
        public KeTerm(string namespaceName, string name, KeTypeReference type)
            : base(namespaceName, name)
        {
        }

        public KeTerm(KeSchema schema, string name, KeTypeReference type)
            : base(schema, name)
        {
        }

        /// <summary>
        /// Gets/sets the declared schema.
        /// </summary>
        public override KeElementKind Kind => KeElementKind.Term;
    }
}
