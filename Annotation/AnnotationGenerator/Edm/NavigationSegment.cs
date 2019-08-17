using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnotationGenerator.Edm
{
    class NavigationSegment : PathSegment
    {
        public NavigationSegment(IEdmNavigationProperty property)
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
    }
}
