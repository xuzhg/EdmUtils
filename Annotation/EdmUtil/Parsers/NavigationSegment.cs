// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    /// <summary>
    /// A navigation property segment.
    /// ~/users/{id}/orders
    /// </summary>
    public class NavigationSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NavigationSegment"/> class.
        /// </summary>
        /// <param name="property">The key/value pairs for the key segment.</param>
        /// <param name="navigationSource">The related navigation source.</param>
        public NavigationSegment(IEdmNavigationProperty property, IEdmNavigationSource navigationSource)
            : this(property, navigationSource, property?.Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NavigationSegment"/> class.
        /// </summary>
        /// <param name="property">The key/value pairs for the key segment.</param>
        /// <param name="navigationSource">The related navigation source.</param>
        /// <param name="identifier">The uri segment literal string.</param>
        public NavigationSegment(IEdmNavigationProperty property, IEdmNavigationSource navigationSource, string identifier)
            : base(identifier)
        {
            NavigationProperty = property ?? throw new ArgumentNullException(nameof(property));
            NavigationSource = navigationSource;
            IsSingle = !property.Type.IsCollection();
            EdmType = property.Type.Definition;
        }

        /// <inheritdoc/>
        public override SegmentKind Kind => SegmentKind.Navigation;

        public IEdmNavigationProperty NavigationProperty { get; }

        public override bool IsSingle { get; }

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get; }

        public override string Target => NavigationProperty.Name;

        /// <summary>
        /// Gets the Uri literal for the navigation segment.
        /// It should be the name of the navigation.
        /// </summary>
        public override string UriLiteral => NavigationProperty.Name;

        /// <inheritdoc/>
        public override bool Match(PathSegment other)
        {
            NavigationSegment otherNavigationSegment = other as NavigationSegment;
            if (otherNavigationSegment == null)
            {
                return false;
            }

            return ReferenceEquals(NavigationProperty, otherNavigationSegment.NavigationProperty);
        }
    }
}
