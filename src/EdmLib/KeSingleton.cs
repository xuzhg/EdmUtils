// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the Singleton.
    /// </summary>
    public class KeSingleton : KeNavigationSource
    {
        public KeSingleton(string name, KeEntityType entityType, KeEntityContainer container)
            : base(name, entityType, container)
        {
        }

        public override KeElementKind Kind => KeElementKind.Singleton;
    }
}
