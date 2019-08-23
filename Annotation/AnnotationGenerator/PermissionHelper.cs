using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Annotation;
using AnnotationGenerator.Serialization;
using AnnotationGenerator.Terms;
using AnnotationGenerator.Vocabulary;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace AnnotationGenerator
{
    internal class PermissionHelper
    {
        private static PermissionsRecord CreateGetPermissionRecord(PathKind pathKind)
        {
            switch (pathKind)
            {
                case PathKind.EntitySet:
                case PathKind.CollectionNavigation:
                    return new ReadRestrictionsType();

                case PathKind.Entity:
                case PathKind.SingleNavigation:
                case PathKind.Singleton:
                    return new ReadRestrictionsType
                    {
                        ReadByKeyRestrictions = new ReadByKeyRestrictions()
                    };

                case PathKind.Operation:
                case PathKind.OperationImport:
                    return new OperationRestrictionsType();

                default:
                    throw new Exception($"Invalid path kind {pathKind} in 'GET'");
            }
        }

        private static PermissionsRecord CreatePatchPermissionRecord(PathKind pathKind)
        {
            switch (pathKind)
            {
                case PathKind.Entity:
                case PathKind.SingleNavigation:
                    return new UpdateRestrictionsType();

                case PathKind.Operation:
                case PathKind.OperationImport:
                case PathKind.Singleton:
                case PathKind.EntitySet:
                case PathKind.CollectionNavigation:
                default:
                    throw new Exception($"Invalid path kind {pathKind} in 'PATCH'");
            }
        }

        private static PermissionsRecord CreatePostPermissionRecord(PathKind pathKind)
        {
            switch (pathKind)
            {
                case PathKind.Operation:
                case PathKind.OperationImport:
                    return new OperationRestrictionsType();

                case PathKind.EntitySet:
                case PathKind.CollectionNavigation:
                    return new InsertRestrictionsType();

                case PathKind.Entity:
                case PathKind.SingleNavigation:
                case PathKind.Singleton:
                default:
                    throw new Exception($"Invalid path kind {pathKind} in 'POST'");
            }
        }

        private static PermissionsRecord CreateDeletePermissionRecord(PathKind pathKind)
        {
            switch (pathKind)
            {
                case PathKind.Entity:
                case PathKind.SingleNavigation:
                    return new DeleteRestrictionsType();

                case PathKind.EntitySet:
                case PathKind.CollectionNavigation:
                case PathKind.Singleton:
                case PathKind.Operation:
                case PathKind.OperationImport:
                default:
                    throw new Exception($"Invalid path kind {pathKind} in 'DELETE'");
            }
        }

        public static PermissionsRecord ConvertToRecord(PathKind pathKind, Permission perm)
        {
            PermissionsRecord record = null;

            switch (perm.HttpVerb)
            {
                case "GET":
                    record = CreateGetPermissionRecord(pathKind);
                    break;
                case "PATCH":
                    record = CreatePatchPermissionRecord(pathKind);
                    break;
                case "POST":
                    record = CreatePostPermissionRecord(pathKind);
                    break;
                case "DELETE":
                    record = CreateDeletePermissionRecord(pathKind);
                    break;
                default:
                    throw new Exception($"Unknown http verb {perm.HttpVerb}");
            }

            if (record == null)
            {
                throw new Exception($"Invalid HttpVerb {perm.HttpVerb} in {pathKind} path");
            }

            ReadRestrictionsType read = record as ReadRestrictionsType;
            if (read != null && read.ReadByKeyRestrictions != null)
            {
                read.ReadByKeyRestrictions.InitializeFrom(perm);
                return read;
            }

            record.InitializeFrom(perm);
            return record;
        }

        public static IDictionary<string, IList<Permission>> Load(string fileName)
        {
            IDictionary<string, IList<Permission>> permissions = new Dictionary<string, IList<Permission>>();
            try
            {
                string json = File.ReadAllText(fileName);
                JObject jObj = JObject.Parse(json);
                foreach (var property in jObj.Properties())
                {
                    JArray array = property.Value as JArray;
                    if (array == null)
                    {
                        throw new Exception($"Not valid format, Need an array value of the property: {property.Name}");
                    }

                    IList<Permission> subPermissions = new List<Permission>();
                    foreach (var item in array)
                    {
                        JObject permissionObj = item as JObject;
                        if (permissionObj == null)
                        {
                            throw new Exception($"Not valid format, Need object value of array in the property: {property.Name}");
                        }

                        Permission permission = permissionObj.ToObject<Permission>();
                        subPermissions.Add(permission);
                    }

                    permissions[property.Name.Trim()] = subPermissions;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return permissions;
            }

            return permissions;
        }
    }
}
