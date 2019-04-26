using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmLib
{
    public abstract class KeType
    {
        public string Namespace { get; set; }

        public string Name { get; set; }

        public KeSchema DeclareSchema { get; set; }
    }

    public class KeTypeReference
    {
        public KeType Type { get; set; }

        public bool IsNullable { get; set; }

        public bool IsCollection { get; set; }
    }
}
