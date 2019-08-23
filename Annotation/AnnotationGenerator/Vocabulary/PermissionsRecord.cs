
using System.Collections;
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

        public virtual void InitializeFrom(Permission permission)
        {
            Permissions = new List<PermissionType>();

            //Dictionary<string, string> map = new Dictionary<string, string>
            //{
            //    { "DelegatedWork", "Delegated (work or school account)" },
            //    { "DelegatedPersonal", "Delegated (personal Microsoft account)" },
            //    { "Application", "Application" }
            //};

            PermissionType p = ConvertFromPerm("Delegated (work or school account)", permission.DelegatedWork);
            if (p != null)
            {
                Permissions.Add(p);
            }

            p = ConvertFromPerm("Delegated (personal Microsoft account)", permission.DelegatedPersonal);
            if (p != null)
            {
                Permissions.Add(p);
            }

            p = ConvertFromPerm("Application", permission.Application);
            if (p != null)
            {
                Permissions.Add(p);
            }
        }

        private static PermissionType ConvertFromPerm(string name, IList<string> scopes)
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

            p.Scopes = scopes.Select(s => new ScopeType
            {
                Scope = s.Trim()
            }).ToList();

            return p;
        }
    }
}
