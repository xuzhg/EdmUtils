// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Microsoft.OData.Utils.Value
{
    internal class MetaBoolean : IMetaValue
    {
        public MetaBoolean(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public MetaValueKind Kind => MetaValueKind.MBoolean;
    }
}