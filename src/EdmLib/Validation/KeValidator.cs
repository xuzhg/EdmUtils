using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmLib.Validation
{
    public class KeValidator
    {
        /*
         * 15.2 Simple Identifier
A simple identifier is a Unicode character sequence with the following restrictions:

It consists of at least one and at most 128 Unicode characters.
The first character MUST be the underscore character (U+005F) or any character in the Unicode category “Letter (L)” or “Letter number (Nl)”.
The remaining characters MUST be the underscore character (U+005F) or any character in the Unicode category “Letter (L)”, “Letter number (Nl)”, “Decimal number (Nd)”, “Non-spacing mark (Mn)”, “Combining spacing mark (Mc)”, “Connector punctuation (Pc)”, and “Other, format (Cf)”.
Non-normatively speaking it starts with a letter or underscore, followed by at most 127 letters, underscores or digits.
*/
        public static bool VerifySimpleIdentifier(string identifier)
        {
            if (String.IsNullOrWhiteSpace(identifier))
            {
                return false;
            }

            if (identifier.Length > 128)
            {
                return false;
            }

            char first = identifier[0];
            if (!Char.IsLetter(first) && first != '_')
            {
                return false;
            }

            for (int i = 1; i < identifier.Length; i++)
            {
                if (!Char.IsLetterOrDigit(identifier[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsUnique(KeSchema schema, string name)
        {
            if (schema.EnumTypes.Any(e => e.Name == name))
            {
                return false;
            }

            if (schema.ComplexTypes.Any(e => e.Name == name))
            {
                return false;
            }

            if (schema.EntityTypes.Any(e => e.Name == name))
            {
                return false;
            }

            return true;
        }
    }
}
