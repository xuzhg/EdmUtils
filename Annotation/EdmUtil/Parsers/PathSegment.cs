// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    /// <summary>
    /// The base class for the segment.
    /// </summary>
    public abstract class PathSegment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PathSegment"/> class.
        /// </summary>
        /// <param name="identifier">The segment identifier.</param>
        public PathSegment(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }

        public abstract bool IsSingle { get;}

        public abstract IEdmType EdmType { get; }

        public abstract IEdmNavigationSource NavigationSource { get; }
    }
}
