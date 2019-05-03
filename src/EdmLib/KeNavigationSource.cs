// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the navigation source.
    /// </summary>
    public abstract class KeNavigationSource : KeElement
    {
        public KeNavigationSource(string name, KeEntityType entityType, KeEntityContainer contaner)
        {
            Name = name;
            Type = entityType;
            DeclaringContainer = contaner;
        }

        /// <summary>
        /// Gets/sets the namespace.
        /// </summary>
        public KeEntityType Type { get; }

        /// <summary>
        /// Gets/sets the navigation source name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets/sets the declared schema.
        /// </summary>
        public KeEntityContainer DeclaringContainer { get; }
    }
}
