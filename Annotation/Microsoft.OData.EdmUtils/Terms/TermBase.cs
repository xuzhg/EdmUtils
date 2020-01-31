// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.OData.EdmUtils.Terms
{
    public interface ITerm
    {
        string Namespace { get; }

        string Name { get; }


    }

}
