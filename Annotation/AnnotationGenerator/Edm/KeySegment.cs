
using System;
using System.Collections.Generic;
using Microsoft.OData.Edm;

namespace AnnotationGenerator.Edm
{
    public class KeySegment : PathSegment
    {
        public KeySegment(string identifier)
            : base(identifier)
        {

        }

        public KeySegment(IDictionary<string, string> keys, PathSegment previous)
            : base("keys")
        {

        }

        public KeySegment(string identifier, PathSegment previous)
            : base(identifier)
        {
            OwnedSegment = previous;
        }

        public PathSegment OwnedSegment { get; }

        public override bool IsSingle => true;

        public override IEdmType EdmType => OwnedSegment.EdmType;

        public override IEdmNavigationSource NavigationSource => OwnedSegment.NavigationSource;
    }
}
