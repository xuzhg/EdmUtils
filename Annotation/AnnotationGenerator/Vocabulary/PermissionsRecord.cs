
using System.Collections.Generic;

namespace AnnotationGenerator.Vocabulary
{
    /// <summary>
    /// Base class for Permission record.
    /// </summary>
    internal class PermissionsRecord
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
    }
}
