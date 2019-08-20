
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    public class PropertySegment : PathSegment
    {
        public PropertySegment(IEdmStructuralProperty property, IEdmNavigationSource source)
            : base(property.Name)
        {
            Property = property;
            IsSingle = !property.Type.IsCollection();
            EdmType = property.Type.Definition;
            NavigationSource = source;
        }

        public IEdmStructuralProperty Property { get; }

        public override bool IsSingle { get; }

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource { get; }
    }
}
