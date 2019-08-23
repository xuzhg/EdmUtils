using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AnnotationGenerator.Serialization
{
    public interface IRecord
    {
        string TermName { get; }
        void Write(XmlWriter writer);
    }
}
