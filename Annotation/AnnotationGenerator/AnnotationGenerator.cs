// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Annotation;
using Annotation.EdmUtil;
using AnnotationGenerator.MD;
using AnnotationGenerator.Serialization;
using AnnotationGenerator.Terms;
using AnnotationGenerator.Vocabulary;
using Microsoft.OData.Edm;

namespace AnnotationGenerator
{
    class AnnotationGenerator : IDisposable
    {
        protected Stream stream;
        protected XmlWriter writer;
        protected IEdmModel model;

        public IDictionary<string, Exception> PermissionsError { get; set; } = new Dictionary<string, Exception>();

        public AnnotationGenerator(string output, IEdmModel model)
        {
            this.model = model;
            stream = new FileStream(output, FileMode.Create);
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            writer = XmlWriter.Create(stream, settings);

            // <Schema>
            writer.WriteStartElement("Schema");
        }


        public void Add(IDictionary<UriPath, IList<ApiPermissionType>> permissions)
        {
            IDictionary<string, IList<IRecord>> targetStringMerged = new Dictionary<string, IList<IRecord>>();
            foreach (var permission in permissions)
            {
                UriPath path = permission.Key;
                PathKind kind = path.Kind;
                string target = path.GetTargetString();

                IList<IRecord> records;
                if (!targetStringMerged.TryGetValue(target, out records))
                {
                    records = new List<IRecord>();
                    targetStringMerged[target] = records;
                }

                foreach (var perm in permission.Value)
                {
                    PermissionsRecord permissionRecord;
                    try
                    {
                        permissionRecord = ApiPermissionHelper.ConvertToRecord(kind, perm);

                        ReadRestrictionsType readRest = permissionRecord as ReadRestrictionsType;
                        if (readRest != null)
                        {
                            var existingReadRest = records.FirstOrDefault(r => r is ReadRestrictionsType);
                            if (existingReadRest != null)
                            {
                                MergeReadRest(existingReadRest as ReadRestrictionsType, readRest, target);
                                continue;
                            }
                        }

                        // TODO: verify only one Restriction existing for one target?

                        records.Add(permissionRecord as IRecord);
                    }
                    catch (Exception ex)
                    {
                        //var color = Console.BackgroundColor;
                        //Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("    [PermssionError]: " + ex.Message);
                        //Console.BackgroundColor = color;

                        PermissionsError[target] = ex;
                    }
                }
            }

            foreach (var item in targetStringMerged)
            {
                Write(item.Key, item.Value);
            }
        }

        public static void MergeReadRest(ReadRestrictionsType existing, ReadRestrictionsType newRead, string target)
        {
            if (existing.ReadByKeyRestrictions != null && newRead.ReadByKeyRestrictions != null)
            {
                throw new Exception($"Found mutltiple read by key restrctions for one target '{target}'.");
            }

            if (existing.ReadByKeyRestrictions == null && newRead.ReadByKeyRestrictions == null)
            {
                throw new Exception($"Found mutltiple read restrctions for one target '{target}'.");
            }

            if (existing.ReadByKeyRestrictions != null)
            {
                existing.Readable = newRead.Readable;

                if (newRead.Permissions != null)
                {
                    foreach (var item in newRead.Permissions)
                    {
                        existing.Append(item);
                    }
                }
            }
            else // newRead.ReadByKeyRestrictions != null
            {
                existing.ReadByKeyRestrictions = newRead.ReadByKeyRestrictions;
            }
        }

        public void Add(IDictionary<string, IList<ApiPermissionsBySchemeType>> permissionsByScheme)
        {
            Console.WriteLine("[ ApiPermissionsByScheme ] starting ...");

            if (permissionsByScheme == null || !permissionsByScheme.Any())
            {
                return;
            }

            if (this.model.EntityContainer == null)
            {
                Console.WriteLine("The EntityContainer is null at current model, cannot apply the permissions by scheme");
                return;
            }

            string entitySetFullName = this.model.EntityContainer.FullName();

            // <Annotations Target="...">
            writer.WriteStartElement("Annotations");
            writer.WriteAttributeString("Target", entitySetFullName);

            // <Annotation Term="Org.OData.Authorization.V1.Authorizationss">
            writer.WriteStartElement("Annotation");
            writer.WriteAttributeString("Term", "Org.OData.Authorization.V1.Authorizations");

            writer.WriteStartElement("Collection");

            foreach (var perm in permissionsByScheme)
            {
                Console.WriteLine("==>" + perm.Key);

                IRecord record;
                try
                {
                    record = ApiPermissionHelper.ConvertToRecord(perm);

                    writer.WriteRecord(record, "Org.OData.Authorization.V1.OAuth2Implicit");
                }
                catch (Exception ex)
                {
                    var color = Console.BackgroundColor;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("    [PermssionError]: " + ex.Message);
                    Console.BackgroundColor = color;
                }
            }

            // </Collection>
            writer.WriteEndElement();

            // </Annotation>
            writer.WriteEndElement();

            // </Annotations>
            writer.WriteEndElement();

            Console.WriteLine("[ ApiPermissionsByScheme ] Done!");
        }

        public void Write(string target, IList<IRecord> records)
        {
            // </Annotations>
            writer.WriteStartElement("Annotations");
            writer.WriteAttributeString("Target", target);

            foreach (var record in records)
            {
                // <Annotation Term="Org.OData.Cap...V1.ReadRestrictions">
                writer.WriteStartElement("Annotation");

                writer.WriteAttributeString("Term", record.TermName);

                writer.WriteRecord(record);

                // </Annotation>
                writer.WriteEndElement();
            }

            // </Annotations>
            writer.WriteEndElement();
        }

        public void Write(ITerm term)
        {

            if (!term.IsInLine)
            {
                // </Annotation>
                writer.WriteStartElement("Annotations");
                writer.WriteAttributeString("Target", term.Target);
            }

            // <Annotation Term="Org.OData.Cap...V1.ReadRestrictions">
            writer.WriteStartElement("Annotation");

            writer.WriteAttributeString("Term", "Org.OData.Capabilities.V1.ReadRestrictions");

            if (term.IsCollection)
            {
                writer.WriteCollection(term.Records);
            }
            else
            {
                writer.WriteRecord(term.Records.First());
            }

            // </Annotation>
            writer.WriteEndElement();

            if (!term.IsInLine)
            {
                // </Annotations>
                writer.WriteEndElement();
            }
        }

        public void SaveAs(string fileName)
        {
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(stream);

            // Removing xml header to make the baseline's more compact and focused on the test at hand.
            string result = reader.ReadToEnd();

            File.WriteAllText(fileName, result);

            stream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            writer = XmlWriter.Create(stream, settings);
        }

        public void Dispose()
        {
            if (writer != null)
            {
                // </Schema>
                writer.WriteEndElement();

                writer.Flush();
                writer.Dispose();
                stream.Dispose();
                writer = null;
                stream = null;
            }
        }
    }
}
