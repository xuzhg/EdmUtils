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
        /// <param name="actualType">The actual type, from the Uri.</param>
        /// <param name="expectedType">The expected type, from the Edm model.</param>
        /// <param name="navigationSource">The navigation source.</param>
        public TypeSegment(IEdmType actualType, IEdmType expectedType, IEdmNavigationSource navigationSource)
            : this(actualType, expectedType, navigationSource, actualType?.FullTypeName())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TypeSegment"/> class.
        /// </summary>
        /// <param name="actualType">The actual type, from the Uri.</param>
        /// <param name="expectedType">The expected type, from the Edm model.</param>
        /// <param name="navigationSource">The navigation source.</param>
        /// <param name="identifier">The request Uri segment literal string.</param>
        public TypeSegment(IEdmType actualType, IEdmType expectedType, IEdmNavigationSource navigationSource, string identifier)
            : base(identifier)
        {
            ActualType = actualType ?? throw new ArgumentNullException(nameof(actualType));
            ExpectedType = expectedType ?? throw new ArgumentNullException(nameof(expectedType));

            NavigationSource = navigationSource;
            IsSingle = actualType.TypeKind != EdmTypeKind.Collection;

            Target = actualType.FullTypeName();
        }

        public IEdmType ActualType { get; }

        public IEdmType ExpectedType { get; }

        public override bool IsSingle { get; }

        public override IEdmType EdmType => ActualType;

        public override IEdmNavigationSource NavigationSource { get;}

        public override string Target { get; }
    }
}
