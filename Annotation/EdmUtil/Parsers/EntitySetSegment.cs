// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    /// <summary>
    /// An entity set segment.
    /// </summary>
    public class EntitySetSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EntitySetSegment"/> class.
        /// </summary>
        /// <param name="entitySet">The wrapped entity set.</param>
        public EntitySetSegment(IEdmEntitySet entitySet)
            : base(entitySet?.Name)
        {
            EntitySet = entitySet ?? throw new ArgumentNullException(nameof(entitySet));
            EdmType = entitySet.Type; // It should be collection

            Target = entitySet.Container.Namespace + "/" + entitySet.Name;
        }

        public IEdmEntitySet EntitySet { get; }

        public override bool IsSingle => false;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource => EntitySet;

        public override string Target { get; }
    }
}
