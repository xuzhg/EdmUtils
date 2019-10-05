// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using AnnotationGenerator.MD;
using AnnotationGenerator.Terms;
using AnnotationGenerator.Vocabulary;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;

namespace AnnotationGenerator
{
    class Program
    {


        // args[0] : the api permssion file, it's json
        // args[1] : the related csdl file, it's xml
        // args[2] : the output file
        static void Main(string[] args)
        {
            IList<InputArg> inputArgs;
            if (args.Length != 3)
            {
                inputArgs = RetrieveAllDocs();
            }
            else
            {
                // read from args
                inputArgs = new List<InputArg>
                {
                    new InputArg
                    {
                        PermissionFileName = args[0],
                        CsdlFileName = args[1],
                        Output = args[2],
                        ErrorOutput = @".\error.txt"
                    }
                };
            }

            foreach (var inputArg in inputArgs)
            {
                ProcessPermission(inputArg);
            }

            Console.WriteLine("Done!");
        }

        private static void ProcessPermission(InputArg inputArg)
        {
            // Load the permission data : Dictionary<string, PermissionType>
            Console.WriteLine($"Processing : {inputArg.PermissionFileName}");
            ApiPermissionsWrapper wrapper = ApiPermissionHelper.LoadAll(inputArg.PermissionFileName);
            if (wrapper != null)
            {
                Console.WriteLine($"Loaded permission successful! Totally: {wrapper.ApiPermissions.Count} + {wrapper.PermissionsByScheme.Count}");
            }
            else
            {
                Console.WriteLine("Read permission failed!");
                return;
            }

            // load csdl file
            Console.WriteLine($"Processing : {inputArg.CsdlFileName}");
            IEdmModel edmModel = LoadEdmModel(inputArg.CsdlFileName);
            if (edmModel != null)
            {
                Console.WriteLine("Loaded CSDL successful!");
            }
            else
            {
                Console.WriteLine("Read CSDL failed!");
                return;
            }

            wrapper.Process(edmModel);

            using (AnnotationGenerator generator = new AnnotationGenerator(inputArg.Output, edmModel))
            {
                // for each ApiPermissionsByScheme
                generator.Add(wrapper.PermissionsByScheme);

                // for each permission data
                generator.Add(wrapper.ApiPermissionsProcessed);

                // for each Uri parse error:
                OutputUriErrors(inputArg.ErrorOutput, wrapper.UriParserError, generator.PermissionsError);
            }
        }

        public static IEdmModel LoadEdmModel(string fileName)
        {
            string csdl = File.ReadAllText(fileName);
            return CsdlReader.Parse(XElement.Parse(csdl).CreateReader());
        }

        private static void OutputUriErrors(string errorOutput, IDictionary<string, Exception> uriParseErrors,
            IDictionary<string, Exception> permissionErrors)
        {
            using (StreamWriter file = new StreamWriter(errorOutput))
            {
                file.WriteLine("***********[[Uri Parse Error]]***********");
                int index = 1;
                foreach (var item in uriParseErrors)
                {
                    string error = index + ") " + item.Key + ":==> " + item.Value.Message;
                    file.WriteLine(error);
                    index++;
                }

                file.WriteLine("\n\n***********[[Permission Parse Error]]***********");
                index = 1;
                foreach (var item in permissionErrors)
                {
                    string error = index + ") " + item.Key + ":==> " + item.Value.Message;
                    file.WriteLine(error);
                    index++;
                }
            }
        }

        private static void GenerateTerm(AnnotationGenerator generator, ITerm term)
        {
            generator.Write(term);

            generator.SaveAs(@"D:\temp\openapi\test.xml");

            term.IsInLine = false;
            generator.Write(term);

            generator.SaveAs(@"D:\temp\openapi\test_Outline.xml");
        }

        class InputArg
        {
            public string PermissionFileName { get; set; }
            public string CsdlFileName { get; set; }
            public string Output { get; set; }
            public string ErrorOutput { get; set; }
        }

        private static IList<InputArg> RetrieveAllDocs()
        {
            string currentPath = Directory.GetCurrentDirectory();
            Console.WriteLine("CurrentDirectory:" + currentPath);
            int start = currentPath.IndexOf(@"\Annotation\AnnotationGenerator");
            currentPath = currentPath.Substring(0, start + 1) + @"docs\";

            IList<InputArg> inputArgs = new List<InputArg>();

            IDictionary<string, string> xmlDics = new Dictionary<string, string>();
            foreach (var xmlFile in Directory.EnumerateFiles(currentPath, "*.xml"))
            {
                FileInfo fileInfo = new FileInfo(xmlFile);
                var items = fileInfo.Name.Split("-");

                // Only process the file name has '-', for example: graph-beta.xml
                if (items.Length == 2)
                {
                    xmlDics[items[1]] = xmlFile;
                }
            }

            foreach (var file in Directory.EnumerateFiles(currentPath, "*.json"))
            {
                FileInfo fileInfo = new FileInfo(file);

                var items = fileInfo.Name.Split("-");
                if (items.Length != 2)
                {
                    Console.WriteLine($"skip {fileInfo.Name} because the file name doesnot have '-'");
                    continue;
                }

                string replaced = items[1].Replace(".json", ".xml");
                if (xmlDics.TryGetValue(replaced, out string value))
                {
                    inputArgs.Add(new InputArg
                    {
                        PermissionFileName = file,
                        CsdlFileName = value,
                        Output = file.Replace(".json", ".out"),
                        ErrorOutput = file.Replace(".json", ".err")
                    });
                }
            }
            return inputArgs;
        }

        private static ITerm CreateDefaultTerm()
        {
            ReadRestrictions term = new ReadRestrictions();

            term.Target = "Ns.Container/Users";
            term.IsInLine = true;

            ReadRestrictionsType record = new ReadRestrictionsType();

            record.ReadByKeyRestrictions = new ReadByKeyRestrictions();

            PermissionType permission = new PermissionType
            {
                SchemeName = "Delegated (work or school account)",
                Scopes = new List<ScopeType>
                {
                    new ScopeType{ Scope = "User.ReadBasic.All"},
                    new ScopeType{ Scope = "User.Read.All"},
                    new ScopeType{ Scope = "User.ReadWrite.All"},
                    new ScopeType{ Scope = "Directory.Read.All"},
                    new ScopeType{ Scope = "Directory.ReadWrite.All"},
                    new ScopeType{ Scope = "Directory.AccessAsUser.All"}
                }
            };
            record.ReadByKeyRestrictions.Append(permission);

            permission = new PermissionType
            {
                SchemeName = "Application",
                Scopes = new List<ScopeType>
                {
                    new ScopeType{ Scope = "User.Read.All"},
                    new ScopeType{ Scope = "User.ReadWrite.All"},
                    new ScopeType{ Scope = "Directory.Read.All"},
                    new ScopeType{ Scope = "Directory.ReadWrite.All"},
                }
            };
            record.ReadByKeyRestrictions.Append(permission);

            term.Records.Add(record);

            return term;
        }
    }

    /*
     // Get /Users
Delegated (work or school account)	User.ReadBasic.All, User.Read.All, User.ReadWrite.All, Directory.Read.All, Directory.ReadWrite.All, Directory.AccessAsUser.All
Delegated (personal Microsoft account)	Not supported.
Application	User.Read.All, User.ReadWrite.All, Directory.Read.All, Directory.ReadWrite.All

     <Annotations Target="NS.Container/Users" >
       <Annotation Term="Org.OData.Cap...V1.ReadRestrictions">
         <Record>
           <PropertyValue Property="Readable" Boolean="true" />  // maybe not
           <PropertyValue Property="ReadByKeyRestrictions" >
             <Record>
                <PropertyValue Property="Permissions" >
                  <Collection>
                    <Record>
                      <PropertyValue Property="SchemeName" String="Delegated (work or school account)" />
                      <PropertyValue Property="Scopes" >
                        <Collection>
                          <Record>
                            <PropertyValue Property="Scope" String="User.ReadBasic.All" />
                          </Record>
                          <Record>
                            <PropertyValue Property="Scope" String="User.Read.All" />
                          </Record>
                          ......
                        </Collection>
                      </PropertyValue>
                    </Record>

                    <Record>
                      <PropertyValue Property="SchemeName" String="Application" />
                      <PropertyValue Property="Scopes" >
                        <Collection>
                          <Record>
                            <PropertyValue Property="Scope" String="User.ReadWrite.All" />
                          </Record>
                          <Record>
                            <PropertyValue Property="Scope" String="User.ReadWrite.All" />
                          </Record>
                          ......
                        </Collection>
                      </PropertyValue>
                    </Record>
                  </Collection>
                </PropertyValue>
             </Record>
           </PropertyValue>
         </Record>
       </Annotation>
     </Annotations>
     */
}
