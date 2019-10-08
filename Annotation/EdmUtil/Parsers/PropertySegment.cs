// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    /// <summary>
    /// A structural property segment, for example: /users/{id}/name
    /// </summary>
    public class PropertySegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PropertySegment"/> class.
        /// </summary>
        /// <param name="property">The wrapped structural property.</param>
        /// <param name="navigationSource">The navigation source.</param>
        public PropertySegment(IEdmStructuralProperty property, IEdmNavigationSource navigationSource)
            : this(property, navigationSource, property?.Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PropertySegment"/> class.
        /// </summary>
        /// <param name="property">The wrapped structural property.</param>
        /// <param name="navigationSource">The navigation source.</param>
        /// <param name="identifier">The uri segment literal string.</param>
        public PropertySegment(IEdmStructuralProperty property, IEdmNavigationSource navigationSource, string identifier)
            : base(identifier)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            IsSingle = !property.Type.IsCollection();
            EdmType = property.Type.Definition;
            NavigationSource = navigationSource;
        }

        /// <summary>
        /// Gets the Edm property assigned to this segment.
        /// </summary>
        public IEdmStructuralProperty Property { get; }

        /// <summary>
        /// Gets a boolean value indicating whether this segment is single value out.
        /// </summary>
        public override bool IsSingle { get; }

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get; }

        public override string Target => Property.Name;
    }
}
