
using System.Collections.Generic;
using Annotation.EdmUtil.Commons;
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    public class TypeSegment : PathSegment
    {
        public TypeSegment(IEdmType actualType, IEdmType expectedType, IEdmNavigationSource navigationSource)
            : base(actualType.FullTypeName())
        {
            ActualType = actualType;
            ExpectedType = expectedType;
            EdmType = actualType;
            NavigationSource = navigationSource;
            IsSingle = actualType.TypeKind != EdmTypeKind.Collection;
        }

        public IEdmType ActualType { get; }

        public IEdmType ExpectedType { get; }

        public override bool IsSingle { get; }

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get;}
    }
}
