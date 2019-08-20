
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    public abstract class PathSegment
    {
        public PathSegment(string identifier)
        {
            Identifier = identifier;
            Previous = null;
            Next = null;
        }

        public string Identifier { get; }

        public abstract bool IsSingle { get;}

        public abstract IEdmType EdmType { get; }

        public abstract IEdmNavigationSource NavigationSource { get; }

        public KeySegment NestedKeySegment { get; set; }

        public PathSegment Previous { get; set; }

        public PathSegment Next { get; set; }
    }
}
