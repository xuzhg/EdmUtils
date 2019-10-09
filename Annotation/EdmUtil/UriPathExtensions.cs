// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Linq;

namespace Annotation.EdmUtil
{
    public static class UriPathExtensions
    {
        public static string GetTargetString(this UriPath path)
        {
            return string.Join("/", path.Segments.Where(s => !(s is KeySegment)).Select(s => s.Target));
        }

        /// <summary>
        /// Compare the right path is equal to the left path.
        /// for example: ~/users/{id}   ==> A
        ///            : ~/users({id})  or ~/users({user-id})  ==> B
        /// A.EqualsTo(B) ==> true
        /// B.EqualsTo(A) ==> true
        /// </summary>
        /// <param name="path">The left path</param>
        /// <param name="other">The right path</param>
        /// <returns>True or false</returns>
        public static bool EqualsTo(this UriPath path, UriPath other)
        {
            if (path == null && other == null)
            {
                return true;
            }

            if (path == null || other == null)
            {
                return false;
            }

            if (path.Count != other.Count)
            {
                return false;
            }

            for (int i = 0; i < path.Count; i++)
            {
                PathSegment originalSegment = path.Segments[i];
                PathSegment otherSegment = other.Segments[i];

                if (!originalSegment.Match(otherSegment))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compare the right path is sub-set of the left path.
        /// for example: ~/users   ==> A
        ///            : ~/users({id})  or ~/users({id})/Name  ==> B
        /// A.StartsWith(B) ==> false
        /// B.StartsWith(A) ==> true
        /// </summary>
        /// <param name="path">The left path</param>
        /// <param name="other">The right path</param>
        /// <returns>True or false</returns>
        public static bool StartsWith(this UriPath path, UriPath other)
        {
            if (path == null || other == null)
            {
                return false;
            }

            if (other.Count > path.Count)
            {
                return false;
            }

            for (int i = 0; i < other.Count; i++)
            {
                PathSegment originalSegment = path.Segments[i];
                PathSegment otherSegment = other.Segments[i];

                if (!originalSegment.Match(otherSegment))
                {
                    return false;
                }
            }

            return true;
        }
    }


}
