// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Xml;

namespace Microsoft.OData.EdmUtils.Terms
{
    public interface IRecord
    {
        string FullTypeName { get; }

        void Write(XmlWriter writer);
    }
}
