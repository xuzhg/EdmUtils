
using System;
using Microsoft.OData.Edm;

namespace AnnotationGenerator.Edm
{
    public class TypeSegment : PathSegment
    {
        public TypeSegment(IEdmStructuredType type)
            : base(type.FullTypeName())
        {
            Type = type;
        }

        public IEdmStructuredType Type { get; }

        public override bool IsSingle
        {
            get
            {
                return true;
            }
        }

        public override IEdmType EdmType => throw new NotImplementedException();

        public override IEdmNavigationSource NavigationSource => throw new NotImplementedException();
    }
}
