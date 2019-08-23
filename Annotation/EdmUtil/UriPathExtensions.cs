using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Annotation.EdmUtil
{
    public static class UriPathExtensions
    {
        public static string GetTargetString(this UriPath path)
        {
            return string.Join("/", path.Segments.Where(s => !(s is KeySegment)).Select(s => s.Target));
        }
    }
}
