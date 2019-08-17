using System;
using System.Collections.Generic;
using System.Text;

namespace AnnotationGenerator.Edm
{
    class KeySegment : PathSegment
    {
        public KeySegment(string identifier)
            : base(identifier)
        {

        }

        public override bool IsSingle => true;
    }
}
