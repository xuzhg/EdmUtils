using System.Collections.Generic;

namespace PermissionsProcessor
{
    public class ApiPermission
        {
            public string HttpVerb { get; set; }
            public List<string> DelegatedWork { get; set; }
            public List<string> DelegatedPersonal { get; set; }
            public List<string> Application { get; set; }

            public ApiPermission()
            {
                DelegatedWork = new List<string>();
                DelegatedPersonal = new List<string>();
                Application = new List<string>();
            }
        }
    
}
