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

        /// <inheritdoc/>
        public override SegmentKind Kind => SegmentKind.Type;

        /// <summary>
        /// Gets the actural type, it's resolved from the request uri.
        /// </summary>
        public IEdmType ActualType { get; }

        /// <summary>
        /// Gets the expected type, it's from the previous segment.
        /// </summary>
        public IEdmType ExpectedType { get; }

        /// <inheritdoc/>
        public override bool IsSingle { get; }

        /// <inheritdoc/>
        public override IEdmType EdmType => ActualType;

        /// <inheritdoc/>
        public override IEdmNavigationSource NavigationSource { get; }

        /// <inheritdoc/>
        public override string Target { get; }

        /// <summary>
        /// Gets the Uri literal for the Type segment.
        /// It should be the name of the Type.
        /// </summary>
        public override string UriLiteral => ActualType.FullTypeName();

        /// <inheritdoc/>
        public override bool Match(PathSegment other)
        {
            TypeSegment otherTypeSegment = other as TypeSegment;
            if (otherTypeSegment == null)
            {
                return false;
            }

            // Compare the key segment using It's declaring type.
            return ReferenceEquals(ActualType, otherTypeSegment.ActualType) &&
                ReferenceEquals(ExpectedType, otherTypeSegment.ExpectedType);
        }
    }
}
