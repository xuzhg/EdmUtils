// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System.Collections.Generic;

namespace Microsoft.OData.Utils.Meta
{
    public class MetaInlineAnnotation : MetaElement
    {
        public string Uri { get; set; }

        public IList<MetaInclude> Include { get; set; }

        public IList<MetaIncludeAnnotation> IncludeAnnotations { get; set; }
    }

}