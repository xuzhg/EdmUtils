// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the function.
    /// </summary>
    public class KeFunction : KeOperation
    {
        public KeFunction(KeSchema schema, string name, bool isBound, KePathExpression entitySetPathExpression, bool isComposable)
            : base(schema, name, isBound, entitySetPathExpression)
        {
            IsComposable = isComposable;
        }

        public override KeElementKind Kind => KeElementKind.Function;

        /// <summary>
        /// Gets a value indicating whether this instance is composable.
        /// </summary>
        public bool IsComposable { get; private set; }
    }
}
