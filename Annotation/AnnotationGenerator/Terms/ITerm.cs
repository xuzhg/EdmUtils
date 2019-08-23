using AnnotationGenerator.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnnotationGenerator.Terms
{
    public interface ITerm
    {
        string Target { get; set; }

        bool IsInLine { get; set; }

        string TermName { get; }

        bool IsCollection { get; }

        IList<IRecord> Records { get; set; }
    }

    internal class TermBase : ITerm
    {
        public string TermName => "Org.OData.Capabilities.V1.ReadRestrictions";

        public string Target { get; set; }

        public bool IsInLine { get; set; }

        public bool IsCollection => false;

        public IList<IRecord> Records { get; set; } = new List<IRecord>();
    }
}
