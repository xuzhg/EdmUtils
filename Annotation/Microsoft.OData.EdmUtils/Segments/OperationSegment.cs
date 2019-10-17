// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OData.Edm;

namespace Microsoft.OData.EdmUtils.Segments
{
    /// <summary>
    /// A bound operation segment, for example: ~/users/Ns.ResetAll(....)
    /// </summary>
    public class OperationSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OperationSegment"/> class.
        /// </summary>
        /// <param name="operation">The wrapped Edm operation (function or action).</param>
        /// <param name="entitySet">The entity set returns from the operation.</param>
        public OperationSegment(IEdmOperation operation, IEdmEntitySetBase entitySet)
            : this(operation, entitySet, operation?.Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="OperationSegment"/> class.
        /// </summary>
        /// <param name="operation">The wrapped Edm operation (function or action).</param>
        /// <param name="entitySet">The entity set returns from the operation.</param>
        /// <param name="literal">The Uri string literal.</param>
        public OperationSegment(IEdmOperation operation, IEdmEntitySetBase entitySet, string literal)
            : base(operation?.Name)
        {
            Operation = operation ?? throw new ArgumentNullException(nameof(operation));

            EdmType = operation.ReturnType?.Definition;

            NavigationSource = entitySet;

            Target = operation.TargetName();

            if (EdmType != null)
            {
                IsSingle = EdmType.TypeKind != EdmTypeKind.Collection;
            }
        }

        /// <inheritdoc/>
        public override SegmentKind Kind => SegmentKind.Operation;

        /// <summary>
        /// Gets the operation.
        /// </summary>
        public IEdmOperation Operation { get; }

        /// <inheritdoc/>
        public override bool IsSingle { get; }

        /// <inheritdoc/>
        public override IEdmType EdmType { get; }

        /// <inheritdoc/>
        public override IEdmNavigationSource NavigationSource { get; }

        /// <inheritdoc/>
        public override string Target { get; }

        /// <summary>
        /// Gets the Uri literal for the operation segment.
        /// It should be the name of the operation.
        /// </summary>
        public override string UriLiteral => Operation.FullName();

        /// <inheritdoc/>
        public override bool Match(PathSegment other)
        {
            OperationSegment otherOperationSegment = other as OperationSegment;
            if (otherOperationSegment == null)
            {
                return false;
            }

            return ReferenceEquals(Operation, otherOperationSegment.Operation);
        }
    }
}
