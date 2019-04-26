using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmLib
{
    public abstract class KeProperty
    {
        public string PropertyName { get; set; }

        public KeType PropertyType { get; set; }
    }

    public class KeStructuralProperty
    {

    }

    public class KeNavigationProperty
    { }
}
