// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
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
        public OperationSegment(IEdmOperation operation)
            : base(operation?.Name)
        {
            Operation = operation ?? throw new ArgumentNullException(nameof(operation));
           // EdmType = entitySet.EntityType();
        }

        public IEdmOperation Operation { get; }

        public override bool IsSingle => false;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get; }
    }
}
