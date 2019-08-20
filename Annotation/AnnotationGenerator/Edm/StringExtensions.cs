
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AnnotationGenerator.Edm
{
    public static class StringExtensions
    {
        // ({id}) or {id}
        // (key1={id})
        // (param1={one},param2={two})
        public static void ExtractKeyValuePairs(this string input, out IDictionary<string, string> pairs)
        {
            pairs = null;
            if (String.IsNullOrEmpty(input))
            {
                return;
            }

            // handle ( .... )
            if (input.StartsWith('('))
            {
                if (input.EndsWith(')'))
                {
                    input = input.Substring(1, input.Length - 2);
                }
                else
                {
                    throw new Exception($"Input string {input} has inconsistent with '(' and ')'");
                }
            }
            else
            {
                if (input.EndsWith(')'))
                {
                    throw new Exception($"Input string {input} has inconsistent with '(' and ')'");
                }
            }

            pairs = new Dictionary<string, string>();

            var items = input.Split(",");
            foreach (var item in items)
            {
                var subItems = item.Split("=");

                if (subItems.Length == 1)
                {
                    if (pairs.TryGetValue(String.Empty, out string value))
                    {
                        throw new Exception($"Invalid string {input}, has multiple items without '='");
                    }

                    pairs[String.Empty] = subItems[0];
                }
                else if (subItems.Length == 2)
                {
                    if (String.IsNullOrEmpty(subItems[0]))
                    {
                        throw new Exception($"Invalid string {input}, has empty key in {item}");
                    }

                    pairs[subItems[0]] = subItems[1];
                }
                else
                {
                    throw new Exception($"Invalid parameter at {item}");
                }
            }
        }

        public static bool IsKeyOrParameterTemplate(this string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }

            if (input[0] == '{' && input[input.Length - 1] == '}')
            {
                return true;
            }

            return false;
        }
    }
}
