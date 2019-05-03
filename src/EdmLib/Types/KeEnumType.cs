// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace EdmLib
{
    /// <summary>
    /// Represents an enum type.
    /// </summary>
    public class KeEnumType : KeType
    {
        public KeEnumType(string namespaceName, string name, string underlyingTypeName, bool isFlags)
            : base(namespaceName, name)
        {
            UnderingType = KeModel.GetPrimitiveType(underlyingTypeName);
            IsFlag = isFlags;
        }

        public override KeElementKind Kind { get; } = KeElementKind.Enum;

        public bool IsFlag { get; set; }

        public KePrimitiveType UnderingType { get; set; }

        public IEnumerable<KeEnumMember> Members { get; set; }
    }

    public class KeEnumMember
    {
        public KeEnumType DeclaringType { get; set; }
    }
}
