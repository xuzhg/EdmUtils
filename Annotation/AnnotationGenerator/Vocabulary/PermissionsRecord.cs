// ------------------------------------------------------------
//  Copyright (c) saxu@microsoft.com.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using AnnotationGenerator.MD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Base class for Permission record.
    /// </summary>
    internal abstract class PermissionsRecord
    {
        /// <summary>
        /// Gets the List of required scopes to invoke an action or function
        /// </summary>
        public IList<PermissionType> Permissions { get; private set; }

        /// <summary>
        /// Append a permission <see cref="PermissionType"/>
        /// </summary>
        /// <param name="permission">The append permission.</param>
        public void Append(PermissionType permission)
        {
            if (Permissions == null)
            {
                Permissions = new List<PermissionType>();
            }

            Permissions.Add(permission);
        }

        public virtual void InitializeFrom(ApiPermissionType permission)
        {
            Permissions = new List<PermissionType>();

            //Dictionary<string, string> map = new Dictionary<string, string>
            //{
            //    { "DelegatedWork", "Delegated (work or school account)" },
            //    { "DelegatedPersonal", "Delegated (personal Microsoft account)" },
            //    { "Application", "Application" }
            //};

            PermissionType p = ConvertFromPerm("Delegated (work or school account)", permission.DelegatedWork, permission.DelegatedWorkRestrictedProperties);
            if (p != null)
            {
                Permissions.Add(p);
            }

            p = ConvertFromPerm("Delegated (personal Microsoft account)", permission.DelegatedPersonal, permission.DelegatedPersonalRestrictedProperties);
            if (p != null)
            {
                Permissions.Add(p);
            }

            p = ConvertFromPerm("Application", permission.Application, permission.ApplicationRestrictedProperties);
            if (p != null)
            {
                Permissions.Add(p);
            }
        }

        private static PermissionType ConvertFromPerm(string name, IList<string> scopes, IDictionary<string, HashSet<string>> restricted)
        {
            if (scopes == null || scopes.Count == 0)
            {
                return null;
            }

            if (scopes.Count == 1 && scopes[0] == "Not supported.")
            {
                return null;
            }

            PermissionType p = new PermissionType
            {
                SchemeName = name
            };

            Func<string, IDictionary<string, HashSet<string>>, string> func = (sc, res) =>
            {
                if (res.TryGetValue(sc, out HashSet<string> value))
                {
                    if (value.Count > 0)
                    {
                        return String.Join(",", value);
                    }
                }

                return null;
            };

            p.Scopes = scopes.Select(s => new ScopeType
            {
                Scope = s.Trim(),
                RestrictedProperties = func(s, restricted)
            }).ToList();

            return p;
        }

        private static PermissionType ConvertFromPerm(string name, IList<PermissionScopeType> scopes)
        {
            if (scopes == null || scopes.Count == 0)
            {
                return null;
            }

            if (scopes.Count == 1 && scopes[0].ScopeName == "Not supported.")
            {
                return null;
            }

            PermissionType p = new PermissionType
            {
                SchemeName = name
            };

            p.Scopes = scopes.Select(s => new ScopeType
            {
                Scope = s.ScopeName.Trim(),
                RestrictedProperties = s.RestrictedProperties.Any() ? String.Join(",", s.RestrictedProperties) : null
            }).ToList();

            return p;
        }
    }
}
