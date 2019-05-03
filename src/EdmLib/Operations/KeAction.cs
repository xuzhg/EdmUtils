// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the action
    /// </summary>
    public class KeAction : KeOperation
    {
        public KeAction(KeSchema schema, string name, bool isBound, KePathExpression entitySetPathExpression)
            : base(schema, name, isBound, entitySetPathExpression)
        {
        }

        public override KeElementKind Kind => KeElementKind.Action;
    }
}
