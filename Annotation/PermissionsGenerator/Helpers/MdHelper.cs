using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PermissionsGenerator.Helpers
{
    class MdHelper
    {
        private static string CleanMarkdown(string i)
        {
            var tableHeaderPattern = "\\|(\\s+)?Permission(s)(\\s+)?\\|.*";

            if (!Regex.IsMatch(i, tableHeaderPattern))
            {
                var ii = Regex.Replace(i, tableHeaderPattern, "");
                var tt = Regex.Replace(ii, "\\|(:)?-+(\\s)?\\|.*", "");

                return tt;
            }
            else { return string.Empty; }
        }

        private static void SplitPermissionInfo(string info, bool delegated)
        {
            if (!info.StartsWith("<p>none", StringComparison.OrdinalIgnoreCase))
            {
                var sp = info.Take(info.LastIndexOf("--|") + 3).ToString().Split('\n');
                Console.WriteLine(sp.First());
                if (delegated)
                {
                    //add to msa dictionary
                }
            }
        }

        private static List<int> GetIndexesByRegex(string content, string patternToSearch)
        {
            var indexList = new List<int>();
            Regex r = new Regex(patternToSearch, RegexOptions.Multiline);
            var allMatches = r.Matches(content);
            try
            {
                indexList = allMatches.AsEnumerable().Select(p => p.Index).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Indexing of H2 crashed");
            }

            return indexList;
        }
    }
    public static class MarkdownExtensions
    {
        public static string clean(this String str)
        {
            if (str.IndexOf("?") > 0) { str = str.Remove(str.IndexOf("?")); }
            return str.Replace("\r", "").Replace("\n", "").Trim();
        }
    }
}
