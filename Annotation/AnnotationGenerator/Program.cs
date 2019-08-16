using AnnotationGenerator.Serialization;
using AnnotationGenerator.Vocabulary;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AnnotationGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load the permission data : Dictionary<string, PermissionType>
            IDictionary<string, PermissionType> permissions = new Dictionary<string, PermissionType>();
            // Key: the request Uri
            // Value: PermissionType

            // Load Edm data

            AnnotationGenerator generator = new AnnotationGenerator(model: null);

            // for each permission data
            foreach (var permission in permissions)
            {
                // Do Uri parser

                // Generator the annotation
            }

            Console.WriteLine("Hello World!");

            GenerateTerm(generator, CreateDefaultTerm());

            Console.WriteLine("Done!");

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
