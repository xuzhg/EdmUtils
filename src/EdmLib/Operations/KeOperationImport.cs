// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the Operation import.
    /// </summary>
    public abstract class KeOperationImport : KeNamedElement
    {
        public KeOperationImport(KeEntityContainer container, string name, KeOperation operation, KeExpression entitySet)
            : base(name)
        {
            Container = container;
            EntitySet = entitySet;
            Operation = operation;
        }

        /// <summary>
        /// Gets the operation.
        /// </summary>
        public KeOperation Operation { get; private set; }

        /// <summary>
        /// Gets the entity set containing entities returned by this operation import.
        /// </summary>
        public KeExpression EntitySet { get; private set; }

        /// <summary>
        /// Gets the container of this operation.
        /// </summary>
        public KeEntityContainer Container { get; private set; }
    }
}
