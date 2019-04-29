// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the "Edm.Type".
    /// </summary>
    public abstract class KeType
    {
        /// <summary>
        /// Gets/sets the namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets/sets the type name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the declared schema.
        /// </summary>
        public KeSchema DeclaredSchema { get; set; }
    }

}
