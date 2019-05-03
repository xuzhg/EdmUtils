// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace EdmLib
{
    public abstract class KeStructureType : KeType
    {
        public KeStructureType(string namespaceName, string name)
            : base(namespaceName, name)
        {

        }

        public IEnumerable<KeProperty> Properties { get; set; }
    }

    public class KeEntityType : KeStructureType
    {
        public KeEntityType(string namespaceName, string name)
            : base(namespaceName, name)
        {

        }

        public override KeElementKind Kind { get; } = KeElementKind.Entity;

    }

    public class KeComplexType : KeStructureType
    {
        public KeComplexType(string namespaceName, string name)
            : base(namespaceName, name)
        {

        }

        public override KeElementKind Kind { get; } = KeElementKind.Complex;

    }
}
