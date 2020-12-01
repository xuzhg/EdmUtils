// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AnnotationGenerator.MD;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.EdmUtils;
using Microsoft.OData.EdmUtils.Segments;

namespace AnnotationGenerator
{
    class Program
    {
        // args[0] : the api permission file, it's json
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
            ApiPermissionsWrapper wrapper = ApiPermissionsWrapper.LoadFromFile(inputArg.PermissionFileName);
            if (wrapper != null)
            {
                Console.WriteLine($"Loaded permission successful! Totally: {wrapper.ApiPermissions.Count} + {wrapper.PermissionsByScheme?.Count}");
            }
            else
            {
                Console.WriteLine("Read permission failed!");
                return;
            }

            // load csdl file
            Console.WriteLine($"Processing : {inputArg.CsdlFileName}");
            IEdmModel edmModel = LoadEdmModel(inputArg.CsdlFileName);

            IEdmModel workloadModel = LoadEdmModel("C:\\temp\\sample.csdl.xml");


        
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

            using (AnnotationWriter annotationWriter = new AnnotationWriter(inputArg.Output, edmModel))
            {
                AnnotationProcessor processor = new AnnotationProcessor(edmModel);
                // these go to the entity container
                var permissionsByScheme = processor.ProcessPermissionsBySchemeType(wrapper.PermissionsByScheme);
                // for each ApiPermissionsByScheme
                annotationWriter.WritePermissionByScheme(wrapper.PermissionsByScheme);

                // for each permission data
                var permissionsByType = processor.ProcessPermissionsByType(wrapper.ApiPermissionsProcessed);

                annotationWriter.WriteAll(permissionsByType);

                foreach (var perms in wrapper.ApiPermissionsProcessed)
                {
                    var path = perms.Key;
                    var url = path.GetTargetString();
                    var last = path.LastSegment;
                    var first = path.FirstSegment.EdmType;

                    if (last.Kind is SegmentKind.Operation)
                    {
                        path.Segments.RemoveAt(path.Segments.Count-1);
                        last = path.LastSegment;
                    }

                    IEdmEntitySet value;
                 
                    var lastitem = last.EdmType;


                    Console.WriteLine(first.AsElementType().FullTypeName() + "-"+url+"---"+lastitem?.AsElementType()?.FullTypeName());

                }


                // for each Uri parse error:
                OutputUriErrors(inputArg.ErrorOutput, wrapper.UriParserError, processor.PermissionsError);
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
        // todo hook up to gmm here
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
                var items = fileInfo.Name.Split('-');

                // Only process the file name has '-', for example: graph-beta.xml
                if (items.Length == 2)
                {
                    xmlDics[items[1]] = xmlFile;
                }
            }

            foreach (var file in Directory.EnumerateFiles(currentPath, "*.json"))
            {
                FileInfo fileInfo = new FileInfo(file);

                var items = fileInfo.Name.Split('-');
                if (items.Length == 1)
                {
                    Console.WriteLine($"skip {fileInfo.Name} because the file name doesnot have '-'");
                    continue;
                }

                string replaced = items[items.Length - 1].Replace(".json", ".xml");
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
                else
                {
                    Console.WriteLine($"skip {fileInfo.Name} because no related xml existing.");
                    continue;
                }
            }
            return inputArgs;
        }
    }

    class InputArg
    {
        public string PermissionFileName { get; set; }
        public string CsdlFileName { get; set; }
        public string Output { get; set; }
        public string ErrorOutput { get; set; }
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
