using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace PermissionsProcessor
{
    public static class Utils
    {
        public static string GetFilename(this string absFilename)
        {
            return absFilename.Substring(absFilename.LastIndexOf("\\") + 1);
        }
        public static List<string> ReadAllFiles(string path)
        {
            var fileList = Directory.GetFiles(path);
            return fileList.ToList();
        }

        public static List<string> ReadAllFilesNamesOnly(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            var t = d.GetFiles("*.md");
            return t.Select(x => x.Name).ToList(); 
        }

        public static string LoadFileContent(string path)
        {
            return File.ReadAllText(path);
        }

        public static string LoadWebContent(string url)
        {
            WebClient wc = new WebClient();
            var content = wc.DownloadString(url).Replace("\n\n", "\n");
            return content;
        }

       
        public static void SaveToFile(string filename, object obj)
        {
            File.WriteAllText(Environment.CurrentDirectory + "\\"+ filename, JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

        public static string GetResourceName(string operationName)
        {
            return operationName.Substring(0, operationName.IndexOf("-"));
        }

        public static string GetOperationName(string file)
        {
            return file.Substring(file.LastIndexOf('\\') + 1).Replace(".md", "");
        }

        public static List<int> GetIndexes(string content, string v)
        {
            var indexList = new List<int>();
            int index = 0;
            indexList.Add(index);
            do
            {
                index = content.IndexOf("## ", index + 2);
                if (index > 0)
                    indexList.Add(index);
            }
            while (index > 0);
            return indexList;
        }
    }
}
