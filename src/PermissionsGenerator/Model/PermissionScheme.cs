using System.Collections.Generic;

namespace PermissionsProcessor
{
    public class PermissionScheme
    {
        public List<PermissionInfo> DelegatedWork { get; set; }
        public List<PermissionInfo> DelegatedPersonal { get; set; }
        public List<PermissionInfo> Application { get; set; }
    }
}
