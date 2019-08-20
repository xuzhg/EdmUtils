
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    public class NavigationSegment : PathSegment
    {
        public NavigationSegment(IEdmNavigationProperty property)
            : base(property.Name)
        {
            Property = property;
        }

        public NavigationSegment(IEdmNavigationProperty property, IEdmNavigationSource source)
            : base(property.Name)
        {
            Property = property;
        }

        public IEdmNavigationProperty Property { get; }


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
