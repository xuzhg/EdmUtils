// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Microsoft.OData.Utils.Value
{
    internal class MetaString : IMetaValue
    {
        public MetaString(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public MetaValueKind Kind => MetaValueKind.MString;

    }
}