// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.OData.Utils.Parser
{
    public abstract class MetaValue
    {
    }



    public class MetaString : MetaValue
    {
        public string Value { get; set; }
    }

    public class MetaBoolean : MetaValue
    {
        public bool Value { get; set; }
    }

    public class MetaResource : Dictionary<string, MetaValue>
    {

    }

    public class MetaCollect : List<MetaValue>
    {

    }
}