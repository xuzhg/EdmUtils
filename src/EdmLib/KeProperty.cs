// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Defines EDM property types.
    /// </summary>
    public enum KePropertyKind
    {
        /// <summary>
        /// Represents a property implementing <see cref="KeStructuralProperty"/>.
        /// </summary>
        Structural,

        /// <summary>
        /// Represents a property implementing <see cref="KeNavigationProperty"/>.
        /// </summary>
        Navigation,
    }

    /// <summary>
    /// Represents a property.
    /// </summary>
    public abstract class KeProperty
    {
        protected KeProperty(KeStructureType declaringType, string name, KeTypeReference type)
        {
            PropertyName = name;
            PropertyType = type;
            DeclaringType = declaringType;
        }

        /// <summary>
        /// Gets the kind of this property.
        /// </summary>
        public abstract KePropertyKind PropertyKind { get; }

        /// <summary>
        /// The property name.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// The property type.
        /// </summary>
        public KeTypeReference PropertyType { get; }

        /// <summary>
        /// Gets the type that this property belongs to.
        /// </summary>
        KeStructureType DeclaringType { get; }
    }

    public class KeStructuralProperty : KeProperty
    {
        public KeStructuralProperty(KeStructureType declaringType, string name, KeTypeReference type)
            : base(declaringType, name, type)
        {
        }

        public override KePropertyKind PropertyKind => KePropertyKind.Structural;
    }

    public class KeNavigationProperty : KeProperty
    {
        public KeNavigationProperty(KeStructureType declaringType, string name, KeTypeReference type, bool contains)
            : base(declaringType, name, type)
        {
            ContainsTarget = contains;
        }

        public override KePropertyKind PropertyKind => KePropertyKind.Navigation;

        public bool ContainsTarget { get; }
    }
}
