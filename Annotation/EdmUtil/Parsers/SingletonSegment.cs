
using Microsoft.OData.Edm;

namespace Annotation.EdmUtil
{
    public class SingletonSegment : PathSegment
    {
        public SingletonSegment(IEdmSingleton singleton)
            : base(singleton.Name)
        {
            Singleton = singleton;
            EdmType = singleton.EntityType();
        }

        public IEdmSingleton Singleton { get; }

        public override bool IsSingle => true;

        public override IEdmType EdmType { get; }

        public override IEdmNavigationSource NavigationSource => Singleton;
    }
}
