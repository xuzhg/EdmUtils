using System;
using System.Collections.Generic;
using System.Text;

namespace AnnotationGenerator.Edm
{
    abstract class PathSegment
    {
        public PathSegment(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }

        public abstract bool IsSingle { get;}
    }
}
