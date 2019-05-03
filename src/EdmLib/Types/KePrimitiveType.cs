// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    public class KePrimitiveType : KeType
    {
        internal KePrimitiveType(KePrimitiveTypeKind primitiveKind)
            : base("Edm", primitiveKind.ToString())
        {
            PrimitiveKind = primitiveKind;
            FullName = this.Namespace + "." + this.Name;
        }

        /// <summary>
        /// Gets the full name of this type.
        /// </summary>
        public string FullName { get; }

        public KePrimitiveTypeKind PrimitiveKind { get; }

        public override KeElementKind Kind => KeElementKind.Primitive;
    }
}
