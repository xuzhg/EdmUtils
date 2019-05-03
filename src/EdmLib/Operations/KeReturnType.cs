// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the operation return.
    /// </summary>
    public class KeReturnType : KeElement
    {
        public KeReturnType(KeTypeReference returnType)
        {
            Type = returnType;
        }

        public KeTypeReference Type { get; }

        public override KeElementKind Kind => KeElementKind.Return;
    }
}
