// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Microsoft.OData.Utils.Value
{
    internal class MetaInt32 : IMetaValue
    {
        public MetaInt32(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public MetaValueKind Kind => MetaValueKind.MInt32;

    }
}