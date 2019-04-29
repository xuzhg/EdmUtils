// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents a property.
    /// </summary>
    public abstract class KeProperty
    {
        /// <summary>
        /// The property name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The property type.
        /// </summary>
        public KeTypeReference PropertyType { get; set; }
    }

    public class KeStructuralProperty : KeProperty
    {
    }

    public class KeNavigationProperty : KeProperty
    { }
}
