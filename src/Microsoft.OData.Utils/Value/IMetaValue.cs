// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Microsoft.OData.Utils.Value
{

    public enum MetaValueKind
    {
        MObject,
        MArray,
        MString,
        MBoolean,
        MInt32
    }

    public interface IMetaValue
    {
        MetaValueKind Kind { get; }
    }
}