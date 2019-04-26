using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmLib
{
    public abstract class KeStructureType : KeType
    {
        public string Namespace { get; set; }

        public string Name { get; set; }
    }

    public class KeEntityType : KeStructureType
    {

    }

    public class KeComplexType : KeStructureType
    {

    }
}
