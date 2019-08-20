// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    /// <summary>
    /// A bound operation import segment, for example: ~/ResetAll(....)
    /// </summary>
    public class OperationImportSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OperationImportSegment"/> class.
        /// </summary>
        /// <param name="operationImport">The wrapped Edm operation import (function import or action import).</param>
        /// <param name="navigationSource">The Edm navigation source.</param>
        public OperationImportSegment(IEdmOperationImport operationImport, IEdmNavigationSource navigationSource)
            : base(operationImport?.Name)
        {
            OperationImport = operationImport ?? throw new ArgumentNullException(nameof(operationImport));

            NavigationSource = navigationSource;
            EdmType = navigationSource?.EntityType();

            if (operationImport.Operation.ReturnType != null)
            {
                IsSingle = !operationImport.Operation.ReturnType.IsCollection();
            }
        }

        public IEdmOperationImport OperationImport { get; }

        public override bool IsSingle { get; } = false;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get; }
    }
}
