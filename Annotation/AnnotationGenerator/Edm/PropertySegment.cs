
using Microsoft.OData.Edm;

namespace AnnotationGenerator.Edm
{
    public class PropertySegment : PathSegment
    {
        public PropertySegment(IEdmStructuralProperty property)
            : base(property.Name)
        {
            Property = property;
        }

        public IEdmStructuralProperty Property { get; }

        public override bool IsSingle
        {
            get
            {
                return true;
            }
        }

        public override IEdmType EdmType => throw new System.NotImplementedException();

        public override IEdmNavigationSource NavigationSource => throw new System.NotImplementedException();
    }
}
