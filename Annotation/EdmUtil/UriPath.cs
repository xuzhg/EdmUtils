using System.Collections.Generic;
using System.Linq;

namespace Annotation.EdmUtil
{
    public class UriPath
    {
        /// <summary>
        /// The segments that make up this path.
        /// </summary>
        private readonly IList<PathSegment> segments;

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

        private static PathKind CalculatePathKind(IList<PathSegment> segments)
        {
            PathSegment lastSegment = segments.Last();
            if (lastSegment is NavigationSegment)
            {
                if (lastSegment.IsSingle)
                {
                    return PathKind.SingleNavigation;
                }

                return PathKind.CollectionNavigation;
            }
            else if (lastSegment is PropertySegment)
            {
                return PathKind.Property;
            }
            else if (lastSegment is OperationSegment)
            {
                return PathKind.Operation;
            }
            else
            {
                int count = segments.Count;
                if (count == 1)
                {
                    if (lastSegment is EntitySetSegment)
                    {
                        return PathKind.EntitySet;
                    }
                    else if (lastSegment is SingletonSegment)
                    {
                        return PathKind.Singleton;
                    }
                    else if (lastSegment is OperationImportSegment)
                    {
                        return PathKind.OperationImport;
                    }

                    throw new System.Exception($"Unknown path kind!");
                }
                else if (count == 2 && lastSegment is KeySegment &&
                    segments[0] is EntitySetSegment)
                {
                    return PathKind.Entity;
                }
                else
                {
                    PathSegment pre = segments[segments.Count - 2];
                    if (pre is NavigationSegment && lastSegment is KeySegment)
                    {
                        return PathKind.SingleNavigation;
                    }

                    throw new System.Exception($"Unknown path kind!");
                }
            }
        }
    }
}
