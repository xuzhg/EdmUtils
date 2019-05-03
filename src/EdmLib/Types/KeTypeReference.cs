// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the refernce of a type.
    /// </summary>
    public class KeTypeReference
    {
        /// <summary>
        /// Gets/sets the type in this reference.
        /// If it's collection, it means the elment type.
        /// </summary>
        public KeType Type { get; set; }

        /// <summary>
        /// Gets/sets the nullable of this reference.
        /// If it's collection, it means the element nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets/sets the collection of this reference.
        /// </summary>
        public bool IsCollection { get; set; }
    }
}
