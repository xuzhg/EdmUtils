
using System.Xml.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;

namespace AnnotationGenerator.Tests
{
    public static class EdmModelHelper
    {
        public static IEdmModel LoadEdmModel(string source)
        {
            string csdl = Resources.GetString(source);
            return CsdlReader.Parse(XElement.Parse(csdl).CreateReader());
        }
    }
}
