// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm;

namespace Microsoft.OData.EdmUtils.Segments
{
    public static class BindingPathHelper
    {
        public static bool MatchBindingPath(IEdmPathExpression bindingPath, IList<PathSegment> parsedSegments)
        {
            List<string> paths = bindingPath.PathSegments.ToList();

            // If binding path only includes navigation property name, it matches.
            if (paths.Count == 1)
            {
                return true;
            }

            int pathIndex = paths.Count - 2; // Skip the last segment which is navigation property name.

            // Match from tail to head.
            for (int segmentIndex = parsedSegments.Count - 1; segmentIndex >= 0; segmentIndex--)
            {
                PathSegment segment = parsedSegments[segmentIndex];

                bool segmentIsNavigationPropertySegment = segment is NavigationSegment;

                // Containment navigation property or complex property in binding path.
                if (segment is PropertySegment || (segmentIsNavigationPropertySegment && segment.NavigationSource is IEdmContainedEntitySet))
                {
                    if (pathIndex < 0 || string.CompareOrdinal(paths[pathIndex], segment.Identifier) != 0)
                    {
                        return false;
                    }

                    pathIndex--;
                }
                else if (segment is TypeSegment)
                {
                    // May need match type if the binding path contains type cast.
                    if (pathIndex >= 0 && paths[pathIndex].Contains("."))
                    {
                        if (string.CompareOrdinal(paths[pathIndex], segment.EdmType.AsElementType().FullTypeName()) != 0)
                        {
                            return false;
                        }

                        pathIndex--;
                    }
                }
                else if (segment is EntitySetSegment
                      || segment is SingletonSegment
                      || segmentIsNavigationPropertySegment)
                {
                    // Containment navigation property in first if statement for NavigationPropertySegment.
                    break;
                }
            }

            // Return true if all the segments in binding path have been matched.
            return pathIndex == -1 ? true : false;
        }
    }
}
