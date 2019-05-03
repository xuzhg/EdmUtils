// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the EntitySet.
    /// </summary>
    public class KeEntitySet : KeNavigationSource
    {
        public KeEntitySet(string name, KeEntityType entityType, KeEntityContainer container)
            : base(name, entityType, container)
        {
        }

        public override KeElementKind Kind => KeElementKind.EntitySet;
    }
}
