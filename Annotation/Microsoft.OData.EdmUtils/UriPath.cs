// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.EdmUtils.Segments;

namespace Microsoft.OData.EdmUtils
{
    public class UriPath
    {
        private string _pathLiteral;

        /// <summary>
        /// The segments that make up this path.
        /// </summary>
        private readonly IList<PathSegment> segments;

        /// <summary>
        /// Initializes a new instance of <see cref="UriPath"/> class.
        /// </summary>
        /// <param name="segments">The segments.</param>
        public UriPath(IEnumerable<PathSegment> segments)
        {
            this.segments = segments.ToList();

            Kind = CalculatePathKind(this.segments);
        }

        public IList<PathSegment> Segments => segments;

        /// <summary>
        /// Gets the first segment in the path.
        /// </summary>
        public PathSegment FirstSegment => this.segments.FirstOrDefault();

        /// <summary>
        /// Get the last segment in the path. Returns null if the path is empty.
        /// </summary>
        public PathSegment LastSegment => this.segments.LastOrDefault();

        /// <summary>
        /// Get the number of segments in this path.
        /// </summary>
        public int Count => this.segments.Count;

        public PathKind Kind { get; }

        private string _targetString;

        public override string ToString()
        {
            if (_pathLiteral == null)
            {
                _pathLiteral = "~/" + string.Join("/", segments.Select(s => s.UriLiteral));
            }

            return _pathLiteral;
        }

        public string TargetString
        {
            get
            {
                if (_targetString == null)
                {
                    _targetString = this.GetTargetString();
                }

                return _targetString;
            }
        }

        private static PathKind CalculatePathKind(IList<PathSegment> segments)
        {
            PathSegment lastSegment = segments.Last();
            if (lastSegment.Kind == SegmentKind.Navigation)
            {
                if (lastSegment.IsSingle)
                {
                    return PathKind.SingleNavigation;
                }

                return PathKind.CollectionNavigation;
            }

            if (lastSegment.Kind == SegmentKind.Property)
            {
                return PathKind.Property;
            }

            if (lastSegment.Kind == SegmentKind.Operation)
            {
                return PathKind.Operation;
            }

            if (lastSegment.Kind == SegmentKind.Type)
            {
                return PathKind.TypeCast;
            }
            else
            {
                int count = segments.Count;
                if (count == 1)
                {
                    if (lastSegment.Kind == SegmentKind.EntitySet)
                    {
                        return PathKind.EntitySet;
                    }
                    else if (lastSegment.Kind == SegmentKind.Singleton)
                    {
                        return PathKind.Singleton;
                    }
                    else if (lastSegment.Kind == SegmentKind.OperationImport)
                    {
                        return PathKind.OperationImport;
                    }

                    throw new System.Exception($"Unknown path kind {lastSegment.Kind} as one segment path.");
                }
                else if (count == 2 && lastSegment.Kind == SegmentKind.Key && segments[0].Kind == SegmentKind.EntitySet)
                {
                    return PathKind.Entity;
                }
                else
                {
                    PathSegment pre = segments[segments.Count - 2];
                    if (pre.Kind == SegmentKind.Navigation && lastSegment.Kind == SegmentKind.Key)
                    {
                        return PathKind.SingleNavigation;
                    }

                    throw new System.Exception($"Unknown path kind {lastSegment.Kind}.");
                }
            }
        }
    }
}
