using System;
using System.Collections.Generic;
using System.Text;

namespace AnnotationGenerator
{
    public class Permission
    {
        public string HttpVerb { get; set; }

        public IList<string> DelegatedWork { get; set; }

        public IList<string> DelegatedPersonal { get; set; }

        public IList<string> Application { get; set; }
    }
}
