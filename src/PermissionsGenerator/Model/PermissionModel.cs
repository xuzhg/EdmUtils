using System.Collections.Generic;

namespace PermissionsProcessor
{
    public class PermissionModel
    {
        public Dictionary<string, List<ApiPermission>> ApiPermissions { get; set; }
        public PermissionScheme PermissionSchemes { get; set; }
        public PermissionModel()
        {
            ApiPermissions = new Dictionary<string, List<ApiPermission>>();
            PermissionSchemes = new PermissionScheme()
            {
                DelegatedWork = new List<PermissionInfo>(),DelegatedPersonal= new List<PermissionInfo>(), Application = new List<PermissionInfo>() 
            };
        }
    }
}
