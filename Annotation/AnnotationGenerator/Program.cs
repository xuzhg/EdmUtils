// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Annotation.EdmUtil;
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
            string permissionFileName;
            string csdlFileName;
            string output;
            if (args.Length != 3)
            {
                string currentPath = Directory.GetCurrentDirectory();
                Console.WriteLine("CurrentDirectory:" + currentPath);
                int start = currentPath.IndexOf(@"\Annotation\AnnotationGenerator");
                currentPath = currentPath.Substring(0, start + 1);

                permissionFileName = currentPath + @"docs\apiPermissionsAndScopes.txt";
                csdlFileName = currentPath + @"docs\graph.v1.0.xml";
                output = currentPath + @"docs\output.xml";
            }
            else
            {
                permissionFileName = args[0];
                csdlFileName = args[1];
                output = args[2];
            }

            // Load the permission data : Dictionary<string, PermissionType>
            /*
            IDictionary<string, IList<ApiPermissionType>> permissions = ApiPermissionHelper.Load(permissionFileName);
            if (permissions != null && permissions != null)
            {
                Console.WriteLine($"Loaded permission successful! Totally: {permissions.Count}");
            }
            else
            {
                Console.WriteLine("Read permission failed!");
                return;
            }*/
            ApiPermissionsWrapper wrapper = ApiPermissionHelper.LoadAll(permissionFileName);
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
            IEdmModel edmModel = LoadEdmModel(csdlFileName);
            if (edmModel != null)
            {
                Console.WriteLine("Loaded CSDL successful!");
            }
            else
            {
                Console.WriteLine("Read CSDL failed!");
                return;
            }

            using (AnnotationGenerator generator = new AnnotationGenerator(output, edmModel))
            {
                // for each ApiPermissionsByScheme
                generator.Add(wrapper.PermissionsByScheme);

                // for each permission data
                foreach (var permission in wrapper.ApiPermissions)
                {
                    Console.WriteLine("==>" + permission.Key);

                    // Do Uri parser
                    var path = ParseRequestUri(permission.Key, edmModel);
                    if (path == null)
                    {
                        continue;
                    }
                    Console.WriteLine("\t Segment Count: " + path.Count);

                    generator.Add(path, permission.Value);
                }

            }

           // GenerateTerm(generator, CreateDefaultTerm());

            Console.WriteLine("Done!");
        }

        public static UriPath ParseRequestUri(string requestUri, IEdmModel model)
        {
            UriPath path;
            try
            {
                path = PathParser.ParsePath(requestUri, model);
            }
            catch
            {
                try
                {
                    path = PathParser.ParsePath(requestUri, model, true);
                }
                catch(Exception innerEx)
                {
                    var color = Console.BackgroundColor;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.WriteLine($" [UriParseError]: '{innerEx.Message}'");
                    Console.BackgroundColor = color;
                    return null;
                }
            }

            return path;
        }

        public static IEdmModel LoadEdmModel(string fileName)
        {
            string csdl = File.ReadAllText(fileName);
            return CsdlReader.Parse(XElement.Parse(csdl).CreateReader());
        }

        private static void GenerateTerm(AnnotationGenerator generator, ITerm term)
        {
            generator.Write(term);

            generator.SaveAs(@"D:\temp\openapi\test.xml");

            term.IsInLine = false;
            generator.Write(term);

            generator.SaveAs(@"D:\temp\openapi\test_Outline.xml");
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
