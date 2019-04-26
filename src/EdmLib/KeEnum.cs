using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmLib
{
    public class KeEnumType : KeType
    {
        public bool IsFlag { get; set; }

        public KePrimitiveType UnderingType { get; set; }

        public IEnumerable<KeEnumMember> Members { get; set; }
    }

    public class KeEnumMember
    {
        public KeEnumType DeclaringType { get; set; }
    }
}
