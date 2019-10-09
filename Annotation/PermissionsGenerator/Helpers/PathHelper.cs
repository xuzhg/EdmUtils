using PermissionsGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PermissionsProcessor
{
    public class PathHelper
    {
        public static List<string> DocumentedGraphPaths { get; set; }

        public static List<string> GetAllPaths(List<string> filelist)
        {
            var pathList = new List<string>();
            foreach (var file in filelist)
            {
                var mdContent = Utils.LoadFileContent(file);
                var list = GetVerb(mdContent);
                pathList.AddRange(list);
            }
            return pathList;
        }

        public static List<string> GetVerb(string mdContent)
        {
            List<int> indexes = Utils.GetIndexes(mdContent, "#");
            var split = MarkdownDeep.Markdown.SplitSections(mdContent);
            var range = new List<string>();
            foreach (var item in split)
            {
                if (item.ToLower().StartsWith("## http request"))
                {
                    var m = Regex.Matches(item, "(GET|POST|PATCH|DELETE) .*");
                    var t = m.Select(n => n.Value.clean()).ToList();
                    range.AddRange(t);
                }
            }
            return range;
        }

        public static void AllDocumentAllPaths(string path)
        {
            var filelist = Utils.ReadAllFiles(path);
            DocumentedGraphPaths = GetAllPaths(filelist);
        }

    }

    
}
