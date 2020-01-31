// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using AnnotationGenerator.MD;
using AnnotationGenerator.Serialization;
using AnnotationGenerator.Vocabulary;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using Microsoft.OData.EdmUtils;

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

        //public static IEdmRecordExpression ConvertToRecord1(KeyValuePair<string, IList<ApiPermissionsBySchemeType>> perm)
        //{
        //    IEdmExpression nameValue = new EdmStringConstant(perm.Key);
        //    IEdmPropertyConstructor propertyName = new EdmPropertyConstructor("Name", nameValue);

        //    IList<IEdmRecordExpression> scopes = new List<IEdmRecordExpression>();
        //    foreach (ApiPermissionsBySchemeType schema in perm.Value)
        //    {
        //        IList<IEdmPropertyConstructor> scopeProperties = new List<IEdmPropertyConstructor>();
        //        if (schema.Name != null)
        //        {
        //            scopeProperties.Add(new EdmPropertyConstructor("Name", new EdmStringConstant(schema.Name)));
        //        }

        //        if (schema.Grant != null)
        //        {
        //            scopeProperties.Add(new EdmPropertyConstructor("Grant", new EdmStringConstant(schema.Grant)));
        //        }

        //        if (schema.Description != null)
        //        {
        //            scopeProperties.Add(new EdmPropertyConstructor("Description", new EdmStringConstant(schema.Description)));
        //        }

        //        scopes.Add(new EdmRecordExpression(scopeProperties));
        //    }

        //    IEdmPropertyConstructor propertyScopes = new EdmPropertyConstructor("Scopes", new EdmCollectionExpression(scopes));

        //    EdmRecordExpression record = new EdmRecordExpression(propertyName, propertyScopes);
        //    return record;
        //}
    }
}
