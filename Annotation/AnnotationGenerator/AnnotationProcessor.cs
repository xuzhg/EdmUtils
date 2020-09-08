using System;
using System.Collections.Generic;
using System.Linq;
using AnnotationGenerator.MD;
using AnnotationGenerator.Serialization;
using AnnotationGenerator.Vocabulary;
using Microsoft.OData.Edm;
using Microsoft.OData.EdmUtils;

namespace AnnotationGenerator
{
    public class AnnotationProcessor 
    {
        private readonly IEdmModel model;
        public IDictionary<string, Exception> PermissionsError { get; set; } = new Dictionary<string, Exception>();

        public AnnotationProcessor(IEdmModel model)
        {
            this.model = model;
        }

        public List<IRecord> ProcessPermissionsBySchemeType(IDictionary<string, IList<ApiPermissionsBySchemeType>> permissionsByScheme)
        {
            Console.WriteLine("[ ApiPermissionsByScheme ] starting ...");
            List<IRecord> records = new List<IRecord>();

            if (permissionsByScheme == null || !permissionsByScheme.Any())
            {
                return records;
            }

            if (this.model.EntityContainer == null)
            {
                Console.WriteLine("The EntityContainer is null at current model, cannot apply the permissions by scheme");
                return records;
            }

            foreach (var perm in permissionsByScheme)
            {
                Console.WriteLine("==>" + perm.Key);

                IRecord record;
                try
                {
                    record = ApiPermissionHelper.ConvertToRecord(perm);
                    records.Add(record);

                }
                catch (Exception ex)
                {
                    //@todo remove all console write
                    var color = Console.BackgroundColor;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("    [PermssionError]: " + ex.Message);
                    Console.BackgroundColor = color;
                }
            }

            return records;
        }

        public IDictionary<string, IList<IRecord>> ProcessPermissionsByType(IDictionary<UriPath, IList<ApiPermissionType>> permissions)
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
                    try
                    {
                        var permissionRecord = ApiPermissionHelper.ConvertToRecord(kind, perm);

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

            return targetStringMerged;

        }

        internal static void MergeReadRest(ReadRestrictionsType existing, ReadRestrictionsType newRead, string target)
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

  
    }
}