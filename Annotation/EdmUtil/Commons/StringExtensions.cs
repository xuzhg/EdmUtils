
using System;
using System.Collections.Generic;
using System.Linq;

namespace Annotation.EdmUtil.Commons
{
    public static class StringExtensions
    {
        public static string ExtractParenthesis(this string identifier, out string parenthesisExpressions)
        {
            // also supports: name(abc)(efg)
            // => name
            // => (abc)(efg)
            parenthesisExpressions = null;

            int parenthesisStart = identifier.IndexOf('(');
            if (parenthesisStart >= 0)
            {
                if (identifier[identifier.Length - 1] != ')')
                {
                    throw new Exception($"Invalid identifier {identifier}, can't find the end character ')'");
                }

                // split the string to grab the identifier and remove the parentheses
                string returnStr = identifier.Substring(0, parenthesisStart);
                parenthesisExpressions = identifier.Substring(parenthesisStart, identifier.Length - returnStr.Length);
                identifier = returnStr;
            }

            return identifier;
        }

        public static void ExtractKeyValuePairs(this string input, out IDictionary<string, string> pairs, out string remaining)
        {
            pairs = new Dictionary<string, string>();
            remaining = null;

            if (String.IsNullOrEmpty(input))
            {
                return;
            }

            // maybe with keys after function parameters
            // .... (...)(...)
            int startIndex = input.IndexOf('(');
            int endIndex = input.IndexOf(')', startIndex + 1);
            string parenthsis = input.Substring(startIndex + 1, endIndex - startIndex - 1);

            if (endIndex != input.Length - 1)
            {
                remaining = input.Substring(endIndex + 1);
            }

            var items = parenthsis.Split(',');
            foreach (var item in items)
            {
                var subItems = item.Split('=');

                if (subItems.Length == 1)
                {
                    if (pairs.TryGetValue(String.Empty, out string value))
                    {
                        throw new Exception($"Invalid string '{input}', has multiple items without '='.");
                    }

                    // VerifyIsKeyOrParameterTemplate(subItems[0]);

                    pairs[String.Empty] = subItems[0].Trim();
                }
                else if (subItems.Length == 2)
                {
                    if (String.IsNullOrEmpty(subItems[0]))
                    {
                        throw new Exception($"Invalid string '{input}', has empty key in '{item}'.");
                    }

                    // VerifyIsKeyOrParameterTemplate(subItems[1]);
                    pairs[subItems[0].Trim()] = subItems[1].Trim();
                }
                else
                {
                    throw new Exception($"Invalid parameter at {item}");
                }
            }
        }

        public static string ToKeyValueString(this IDictionary<string, string> pairs)
        {
            if (pairs.Count == 1)
            {
                if (pairs.Keys.ElementAt(0) == String.Empty)
                {
                    return pairs.Values.ElementAt(0);
                }
            }

            return String.Join(",", pairs.Select(p => p.Key + "=" + p.Value));
        }

        public static void VerifyIsKeyOrParameterTemplate(this string input)
        {
            int index = input.IndexOf('\''); // enum value template
            if (index > 0)
            {
                if (index == input.Length - 1 ||
                    input[index + 1] != '{' ||
                    !input.EndsWith("}\'"))
                {
                    throw new Exception($"Invalid enum value template '{input}'.");
                }

                return;
            }

            if (String.IsNullOrEmpty(input) ||
                (input[0] != '{' && input[input.Length - 1] != '}'))
            {
                throw new Exception($"Invalid value template '{input}', it must wrap with {{ and }}");
            }
        }

        public static void VerifyIsKeyValueTemplate(this string input)
        {
            if (String.IsNullOrEmpty(input) ||
                (input[0] != '{' && input[input.Length - 1] != '}'))
            {
                throw new Exception($"Invalid key value template '{input}', it must wrap with {{ and }}");
            }
        }
    }
}
