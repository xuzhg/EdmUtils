// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the Action import.
    /// </summary>
    public class KeActionImport : KeOperationImport
    {
        public KeActionImport(KeEntityContainer container, string name, KeAction action, KeExpression entitySetExpression)
            : base(container, name, action, entitySetExpression)
        {
        }

        public override KeElementKind Kind => KeElementKind.ActionImport;
    }
}
