// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm;

namespace Microsoft.OData.EdmUtils.Segments
{
    /// <summary>
    /// The base class for the segment.
    /// </summary>
    public abstract class PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PathSegment"/> class.
        /// </summary>
        /// <param name="identifier">The segment identifier, the string literal from Request Uri.</param>
        public PathSegment(string identifier)
        {
            // It can be anything.
            Identifier = identifier;
        }

        /// <summary>
        /// Gets the Uri string literal from Request Uri, it maybe case sensitive, and with other key patterns.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Gets the normal uri literal, that's the case insensitive Uri literal.
        /// </summary>
        public abstract string UriLiteral { get; }

        /// <summary>
        /// Gets the segment kind.
        /// </summary>
        public abstract SegmentKind Kind { get; }

        /// <summary>
        /// Gets a value indicating whether the output value is single value or collection value of this segment.
        /// </summary>
        public abstract bool IsSingle { get;}

        /// <summary>
        /// Gets the target Edm type of this segment.
        /// </summary>
        public abstract IEdmType EdmType { get; }

        /// <summary>
        /// Gets the target navigation source of this segment.
        /// </summary>
        public abstract IEdmNavigationSource NavigationSource { get; }

        /// <summary>
        /// Gets the target string of this segment.
        /// </summary>
        public abstract string Target { get; }

        /// <summary>
        /// Compare the input segment with this segment.
        /// </summary>
        /// <param name="other">The segment to compare</param>
        /// <returns>true for same, false for non-same</returns>
        public abstract bool Match(PathSegment other);
    }
}
