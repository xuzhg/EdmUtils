// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    /// <summary>
    /// A type cast segment. for example: ~/users/NS.VipUser
    /// </summary>
    public class TypeSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TypeSegment"/> class.
        /// </summary>
        /// <param name="actualType">The actual type.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="navigationSource">The navigation source.</param>
        public TypeSegment(IEdmType actualType, IEdmType expectedType, IEdmNavigationSource navigationSource)
            : base(actualType?.FullTypeName())
        {
            ActualType = actualType ?? throw new ArgumentNullException(nameof(actualType));
            ExpectedType = expectedType ?? throw new ArgumentNullException(nameof(expectedType));
            EdmType = actualType;
            NavigationSource = navigationSource;
            IsSingle = actualType.TypeKind != EdmTypeKind.Collection;
        }

        public IEdmType ActualType { get; }

        public IEdmType ExpectedType { get; }

        public override bool IsSingle { get; }

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get;}
    }
}
