
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    public class EntitySetSegment : PathSegment
    {
        public EntitySetSegment(IEdmEntitySet entitySet)
            : base(entitySet.Name)
        {
            EntitySet = entitySet;
            EdmType = entitySet.EntityType();
        }

        public IEdmEntitySet EntitySet { get; }

        public override bool IsSingle => false;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource => EntitySet;
    }
}
