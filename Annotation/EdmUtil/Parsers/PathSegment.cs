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

        public abstract string Target { get; }
    }
}
