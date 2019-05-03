// Copyright (c) Zhigang Xu.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace EdmLib
{
    /// <summary>
    /// Represents the function import.
    /// </summary>
    public class KeFunctionImport : KeOperationImport
    {
        public KeFunctionImport(KeEntityContainer container, string name, KeFunction function, KeExpression entitySetExpression, bool includeInServiceDocument)
            : base(container, name, function, entitySetExpression)
        {
            IncludeInServiceDocument = includeInServiceDocument;
        }

        public override KeElementKind Kind => KeElementKind.FunctionImport;

        public bool IncludeInServiceDocument { get; }
    }
}
