using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnotationGenerator.Edm
{
    class SingletonSegment : PathSegment
    {
        public SingletonSegment(IEdmSingleton singleton)
            : base(singleton.Name)
        {
            Singleton = singleton;
        }

        public IEdmSingleton Singleton { get; }

        public override bool IsSingle => true;
    }
}
