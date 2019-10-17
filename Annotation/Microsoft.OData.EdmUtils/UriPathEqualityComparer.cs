// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace Microsoft.OData.EdmUtils
{
    public sealed class UriPathEqualityComparer : EqualityComparer<UriPath>
    {
        public override bool Equals(UriPath x, UriPath y)
        {
            return x.EqualsTo(y);
        }

        public override int GetHashCode(UriPath path)
        {
            if (path == null)
            {
                return 0;
            }

            return path.ToString().GetHashCode();
        }
    }
}
