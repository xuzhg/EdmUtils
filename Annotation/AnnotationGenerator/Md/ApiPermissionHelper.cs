// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Annotation;
using AnnotationGenerator.MD;
using AnnotationGenerator.Serialization;
using AnnotationGenerator.Vocabulary;
using Newtonsoft.Json.Linq;

namespace AnnotationGenerator
{
    internal class ApiPermissionHelper
    {
        private static PermissionsRecord CreateGetPermissionRecord(PathKind pathKind)
        {
            switch (pathKind)
            {
                case PathKind.EntitySet:
                case PathKind.CollectionNavigation:
                case PathKind.Property:
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
                    throw new Exception($"Invalid path kind '{pathKind}' in 'GET'");
            }
        }

        private static PermissionsRecord CreatePatchPermissionRecord(PathKind pathKind)
        {
            switch (pathKind)
            {
                case PathKind.Entity:
                case PathKind.SingleNavigation:
                case PathKind.Property:
                    return new UpdateRestrictionsType();

                case PathKind.Operation:
                case PathKind.OperationImport:
                case PathKind.Singleton:
                case PathKind.EntitySet:
                case PathKind.CollectionNavigation:
                default:
                    throw new Exception($"Invalid path kind '{pathKind}' in 'PATCH'");
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
                case PathKind.Property:
                default:
                    throw new Exception($"Invalid path kind '{pathKind}' in 'POST'");
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
                case PathKind.Property:
                default:
                    throw new Exception($"Invalid path kind '{pathKind}' in 'DELETE'");
            }
        }

        public static PermissionsRecord ConvertToRecord(PathKind pathKind, ApiPermissionType perm)
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

        public static IRecord ConvertToRecord(KeyValuePair<string, IList<ApiPermissionsBySchemeType>> perm)
        {
            OAuth2ImplicitType record = new OAuth2ImplicitType();

            record.Name = perm.Key;
            record.Scopes = new List<AuthorizationScopeType>();

            foreach(ApiPermissionsBySchemeType sche in perm.Value)
            {
                AuthorizationScopeType scope = new AuthorizationScopeType();
                scope.Description = sche.Description;
                scope.Grant = sche.Grant;
                scope.Scope = sche.Name;

                record.Scopes.Add(scope);
            }
            
            return record;
        }

        public static IDictionary<string, IList<ApiPermissionType>> Load(string fileName)
        {
            IDictionary<string, IList<ApiPermissionType>> permissions = new Dictionary<string, IList<ApiPermissionType>>();
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

                    IList<ApiPermissionType> subPermissions = new List<ApiPermissionType>();
                    foreach (var item in array)
                    {
                        JObject permissionObj = item as JObject;
                        if (permissionObj == null)
                        {
                            throw new Exception($"Not valid format, Need object value of array in the property: {property.Name}");
                        }

                        ApiPermissionType permission = permissionObj.ToObject<ApiPermissionType>();
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

        public static ApiPermissionsWrapper LoadAll(string fileName)
        {
            ApiPermissionsWrapper wrapper = new ApiPermissionsWrapper();
            try
            {
                string json = File.ReadAllText(fileName);
                JObject jObj = JObject.Parse(json);

                // ApiPermissions
                JProperty apiPermProperty = jObj.Property("ApiPermissions");
                if (apiPermProperty == null)
                {
                    throw new Exception($"Invalid format, Need a top level property named 'ApiPermissions'.");
                }

                JObject apiPermPropertyValue = apiPermProperty.Value as JObject;
                if (apiPermPropertyValue == null)
                {
                    throw new Exception($"Invalid format, Need an object value of the property: 'ApiPermissions'.");
                }

                wrapper.ApiPermissions = new Dictionary<string, IList<ApiPermissionType>>();
                foreach (var property in apiPermPropertyValue.Properties())
                {
                    JArray array = property.Value as JArray;
                    if (array == null)
                    {
                        throw new Exception($"Invalid format, Need an array value of the property: {property.Name}");
                    }

                    IList<ApiPermissionType> subPermissions = new List<ApiPermissionType>();
                    foreach (var item in array)
                    {
                        JObject permissionObj = item as JObject;
                        if (permissionObj == null)
                        {
                            throw new Exception($"Not valid format, Need object value of array in the property: {property.Name}");
                        }

                        ApiPermissionTypeInternal permission = permissionObj.ToObject<ApiPermissionTypeInternal>();

                        subPermissions.Add(ConvertTo(permission));
                    }

                    wrapper.ApiPermissions[property.Name.Trim()] = subPermissions;
                }

                // PermissionsByScheme
                JProperty permissionsByScheme = jObj.Property("PermissionsByScheme");
                if (apiPermProperty == null)
                {
                    throw new Exception($"Invalid format, Need a top level property named 'permissionsByScheme'.");
                }

                JObject permissionsBySchemeValue = permissionsByScheme.Value as JObject;
                if (permissionsBySchemeValue == null)
                {
                    throw new Exception($"Invalid format, Need an object value of the property: 'PermissionsByScheme'.");
                }

                wrapper.PermissionsByScheme = new Dictionary<string, IList<ApiPermissionsBySchemeType>>();
                foreach (var property in permissionsBySchemeValue.Properties())
                {
                    JArray array = property.Value as JArray;
                    if (array == null)
                    {
                        throw new Exception($"Invalid format, Need an array value of the property: {property.Name}");
                    }

                    IList<ApiPermissionsBySchemeType> subPermissionsByScheme = new List<ApiPermissionsBySchemeType>();
                    foreach (var item in array)
                    {
                        JObject subPermissionsBySchemeObj = item as JObject;
                        if (subPermissionsBySchemeObj == null)
                        {
                            throw new Exception($"Not valid format, Need object value of array in the property: {property.Name}");
                        }

                        ApiPermissionsBySchemeType subPermObject = subPermissionsBySchemeObj.ToObject<ApiPermissionsBySchemeType>();
                        subPermissionsByScheme.Add(subPermObject);
                    }

                    wrapper.PermissionsByScheme[property.Name.Trim()] = subPermissionsByScheme;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return wrapper;
        }

        private class ApiPermissionTypeInternal
        {
            public string HttpVerb { get; set; }

            public IList<string> DelegatedWork { get; set; }

            public IList<string> DelegatedPersonal { get; set; }

            public IList<string> Application { get; set; }
        }

        private static ApiPermissionType ConvertTo(ApiPermissionTypeInternal permInternal)
        {
            ApiPermissionType wrapper = new ApiPermissionType();
            wrapper.HttpVerb = permInternal.HttpVerb;

            wrapper.DelegatedWork = permInternal.DelegatedWork.Where(d => d.Trim() != "Not supported.").Select(d => new PermissionScopeType { ScopeName = d.Trim() }).ToList();
            wrapper.DelegatedPersonal = permInternal.DelegatedPersonal.Where(d => d.Trim() != "Not supported.").Select(d => new PermissionScopeType { ScopeName = d.Trim() }).ToList();
            wrapper.Application = permInternal.Application.Where(d => d.Trim() != "Not supported.").Select(d => new PermissionScopeType { ScopeName = d.Trim() }).ToList();

            return wrapper;
        }
    }
}
