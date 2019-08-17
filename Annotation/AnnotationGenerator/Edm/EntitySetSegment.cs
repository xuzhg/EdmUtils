using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnotationGenerator.Edm
{
    class EntitySetSegment : PathSegment
    {
        public EntitySetSegment(IEdmEntitySet entitySet)
            : base(entitySet.Name)
        {
            EntitySet = entitySet;
        }

        public IEdmEntitySet EntitySet { get; }

        public override bool IsSingle => false;
    }
}
