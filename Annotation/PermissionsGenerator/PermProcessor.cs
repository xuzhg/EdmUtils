using Markdig;
using Markdig.Parsers.Inlines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static PermissionsProcessor.Program;

namespace PermissionsProcessor
{
    public class PermProcessor
    {
        public string DirPath { get; internal set; }
        public string ReferencePath { get; internal set; }

        public void GeneratePermissionsReferenceList()
        {
            List<PermissionInfo> delegatedWorkScopes = new List<PermissionInfo>();
            List<PermissionInfo> delegatedPersScopes = new List<PermissionInfo>();
            List<PermissionInfo> appScopes = new List<PermissionInfo>();
            var permRefContent = Utils.LoadFileContent(ReferencePath).Split("\r\n");
            foreach (var item in permRefContent)
            {
                if (Regex.IsMatch(item, "^\\|(\\s+)?_[\\w].*_(\\s+)?\\|"))
                {
                    if (item.Count(f => f == '|') == 6)
                    {
                        var permInfoLine = item.TrimStart('|').TrimEnd('|').Split('|');
                        var delWorkScopeInfo = new PermissionInfo
                        {
                            Name = permInfoLine[0].Replace("_", "").Trim(),
                            Description = permInfoLine[2].Trim(),
                            Grant = permInfoLine[3].Trim().ToLower() == "yes" ? "admin" : "user"
                        };
                        delegatedWorkScopes.Add(delWorkScopeInfo);

                        if (permInfoLine[4].ToLower().Contains("yes"))
                        {
                            var delPersScopeInfo = new PermissionInfo
                            {
                                Name = permInfoLine[0].Replace("_", "").Trim(),
                                Description = permInfoLine[2].Trim(),
                                Grant = permInfoLine[3].Trim().ToLower() == "yes" ? "admin" : "user"
                            };
                            delegatedPersScopes.Add(delPersScopeInfo);
                        }
                    }
                    else
                    {
                        var permInfoLine = item.TrimStart('|').TrimEnd('|').Split('|');
                        var appScopeInfo = new PermissionInfo
                        {
                            Name = permInfoLine[0].Replace("_", "").Trim(),
                            Description = permInfoLine[2].Trim(),
                            Grant = permInfoLine[3].Trim().ToLower() == "yes" ? "admin" : "user"
                        };
                        appScopes.Add(appScopeInfo);
                    }
                }
            }

            PermissionsModel.PermissionSchemes.DelegatedWork = delegatedWorkScopes;
            PermissionsModel.PermissionSchemes.DelegatedPersonal = delegatedPersScopes;
            PermissionsModel.PermissionSchemes.Application= appScopes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void AllPermissionsByPages()
        {
            XDocument xDocument;
            var files = Utils.ReadAllFiles(DirPath);
            foreach (var file in files)
            {
                var mdContent = Utils.LoadFileContent(file);
                mdContent = mdContent.Replace("## Prerequisites", "## Permissions");
                var mdig = Markdown.ToHtml(mdContent);
                var filename = file.GetFilename();
                PermissionsByPages.Add(filename, new ApiPermission());

                try
                {
                    xDocument = XDocument.Parse("<root>" + mdig + "</root>");
                    var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                    var result = Markdown.ToHtml(mdContent, pipeline);
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(result);
                    var permissionsTableData = doc.DocumentNode.SelectSingleNode("/h2[@id='permissions']/following::table/tbody");
                    var permissionsByScheme = permissionsTableData
                                .Descendants("tr")
                                .Select(tr => tr.Elements("td").Skip(1).Select(td => td.InnerText.Split(',').Select(i => i.Trim())));

                    ApiPermission apiPermission = new ApiPermission
                    {
                        DelegatedWork = permissionsByScheme.ElementAt(0).ElementAt(0).ToList(),
                        DelegatedPersonal = permissionsByScheme.ElementAt(1).ElementAt(0).ToList(),
                        Application = permissionsByScheme.ElementAt(2).ElementAt(0).ToList(),
                    };

                    PermissionsByPages[filename] = apiPermission;
                }
                catch (Exception)
                {
                    Console.WriteLine("UnProcessed:" + file);
                }
            }
            //Console.WriteLine(JsonConvert.SerializeObject(PermissionsByPages));
        }

        public void MergePathPermissionData()
        {
            var verbRegexPattern = "^(GET|POST|PATCH|PUT|DELETE)\\s";
            foreach (var page in PathsInPages)
            {
                if (!page.Key.StartsWith("intune-"))
                {
                    foreach (var apipath in page.Value)
                    {
                        var t = PermissionsByPages[page.Key];
                        var verb = Regex.Matches(apipath, verbRegexPattern);
                        t.HttpVerb = verb.First().Value.Trim();
                        var newPath = Regex.Replace(apipath, verbRegexPattern, "");
                        newPath = Regex.Replace(newPath, "\\s", "").ToLower();
                        //if the path exits then add info
                        if (t.Application.Count > 0 & t.DelegatedPersonal.Count > 0 & t.DelegatedWork.Count > 0)
                        {
                            if (PermissionsDataValidator(t))
                            {
                                AddOrMergePermission(t, newPath);
                            }
                        }
                    }
                }
            }
        }

        private bool PermissionsDataValidator(ApiPermission t)
        {
            foreach (var item in t.DelegatedWork)
            {
                if (!PermissionsModel.PermissionSchemes.DelegatedWork.Exists(i => i.Name==item) && item.ToLower()=="not supported")
                {
                    return false;
                }
            }

            foreach (var item in t.DelegatedPersonal)
            {
                if (!PermissionsModel.PermissionSchemes.DelegatedPersonal.Exists(i => i.Name == item) && item.ToLower() == "not supported")
                {
                    return false;
                }
            }

            foreach (var item in t.Application)
            {
                if (!PermissionsModel.PermissionSchemes.Application.Exists(i => i.Name == item) && item.ToLower() == "not supported")
                {
                    return false;
                }
            }
            return true;
        }

        public void AllPathsByPages()
        {
            XDocument xDocument;
            var files = Utils.ReadAllFiles(DirPath);
            foreach (var file in files)
            {
                var mdContent = Utils.LoadFileContent(file);
                var mdig = Markdown.ToHtml(mdContent);
                var filename = file.GetFilename();
                PathsInPages.Add(filename, new List<string>());
                try
                {
                    xDocument = XDocument.Parse("<root>" + mdig + "</root>");
                    var allCodeNodes = xDocument.Descendants().Where(i => i.Name == "code");
                    //PipeTableParser pipeTableParser = new PipeTableParser(new LineBreakInlineParser(), null);
                    var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                    var result = Markdown.ToHtml(mdContent, pipeline);
                    var langhttpCollection = allCodeNodes.Attributes().Where(i => i.Value == "language-http").Select(i => i.Parent);
                    foreach (var item in langhttpCollection)
                    {
                        foreach (var request in item.Value.Split('\n'))
                        {
                            var urlMatch = Regex.Match(request, "^(GET|POST|PATCH|PUT|DELETE)\\s.*");
                            if (urlMatch.Success & !urlMatch.Value.Contains("..."))
                            {
                                var apiPathValue = Regex.Replace(urlMatch.Value, "\\?.*", "");
                                apiPathValue = Regex.Replace(apiPathValue, "https://graph.microsoft.com/(v1.0|beta)", "");
                                PathsInPages[filename] = PathsInPages[filename].Union(new List<string>() { apiPathValue });
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private static void AddOrMergePermission(ApiPermission permission, string apipath)
        {
            if (apipath.StartsWith("https://"))
            {
                return;
            }

            if (PermissionsModel.ApiPermissions.ContainsKey(apipath))
            {
                bool keyExists = false;
                foreach (var item in PermissionsModel.ApiPermissions[apipath])
                {
                    if (item.HttpVerb == permission.HttpVerb)
                    {
                        item.Application.Union(permission.Application);
                        item.DelegatedPersonal.Union(permission.DelegatedPersonal);
                        item.DelegatedWork.Union(permission.DelegatedWork);
                        keyExists = true;
                    }
                }

                if (!keyExists)
                    PermissionsModel.ApiPermissions[apipath].Add(permission);
            }
            else
            {
                var PermissionList = new List<ApiPermission> { permission };
                PermissionsModel.ApiPermissions.Add(apipath, PermissionList);
            }
        }
    }  
}