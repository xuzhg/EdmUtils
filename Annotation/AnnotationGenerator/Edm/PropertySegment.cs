using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnotationGenerator.Edm
{
    class PropertySegment : PathSegment
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

    }
}
