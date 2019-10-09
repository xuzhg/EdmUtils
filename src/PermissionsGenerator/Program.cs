using CommonMark.Syntax;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Parsers;
using Markdig.Parsers.Inlines;
using Newtonsoft.Json;
using PermissionsProcessor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Schema;

namespace PermissionsProcessor
{
    public partial class Program
    {
        public static string outputFilename = "";
        public static PermissionModel PermissionsModel { get; set; } = new PermissionModel();
        public static Dictionary<string, IEnumerable<string>> PathsInPages { get; set; } = new Dictionary<string, IEnumerable<string>>();
        public static Dictionary<string, ApiPermission> PermissionsByPages { get; set; } = new Dictionary<string, ApiPermission>();

        static void Main()
        {
            string BasePath = @"C:\microsoft-graph-docs";
            PermProcessor permProcessor = new PermProcessor()
            {
                ReferencePath = BasePath + @"\concepts\permissions-reference.md"
            };

            Console.WriteLine("Enter 1 for 'v10' or 2 for 'beta'");
            var cmd = Console.ReadLine();
            switch (cmd)
            {
                case "1":
                    permProcessor.DirPath = BasePath+ @"\api-reference\v1.0\api";
                    outputFilename = "v1.0";
                    break;
                case "2":
                    permProcessor.DirPath = BasePath + @"\api-reference\beta\api";
                    outputFilename = "beta";
                    break;
            }

            permProcessor.AllPathsByPages();
            permProcessor.AllPermissionsByPages();
            permProcessor.GeneratePermissionsReferenceList();
            permProcessor.MergePathPermissionData();
            Utils.SaveToFile($"apiPermissionsAndScopes-{outputFilename}.json", PermissionsModel);
            Console.WriteLine("done");
        }
    }
}
