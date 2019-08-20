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
        public NavigationSegment(IEdmNavigationProperty property)
            : base(property.Name)
        {
            Property = property;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NavigationSegment"/> class.
        /// </summary>
        /// <param name="property">The key/value pairs for the key segment.</param>
        /// <param name="navigationSource">The related navigation source.</param>
        public NavigationSegment(IEdmNavigationProperty property, IEdmNavigationSource navigationSource)
            : base(property?.Name)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            NavigationSource = navigationSource;
            IsSingle = !property.Type.IsCollection();
            EdmType = property.Type.Definition;
        }

        public IEdmNavigationProperty Property { get; }

        public override bool IsSingle { get; }

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get; }
    }
}
