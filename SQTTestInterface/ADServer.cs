using System;
using System.Collections;
using System.DirectoryServices.AccountManagement;

namespace SQTTestInterface {
    public class ADServer {
        public string ipAddress { get; }
        private PrincipalContext activeDirectoryPrincipalContext { get; set; }
        public ADServer(string ipIn, string adCred, string pwd) {
            ipAddress = ipIn;
            try {
                activeDirectoryPrincipalContext = new PrincipalContext(ContextType.Domain, ipIn, adCred, pwd);
            } catch {
                throw new NullReferenceException();
            }
        }
        //Group type is 'da' for domain admin,'sa' for server admin, and 'wa' for workstation admin, 
        public bool ValidateCredentials(string adCred, string pwd,string userType) {

            GroupPrincipal currentSearchGroup;
            if (activeDirectoryPrincipalContext.ValidateCredentials(adCred, pwd)) {
                switch (userType) {
                    case "da":
                        currentSearchGroup = GroupPrincipal.FindByIdentity(activeDirectoryPrincipalContext, "Domain Admins");
                        break;
                    case "sa":
                        currentSearchGroup = GroupPrincipal.FindByIdentity(activeDirectoryPrincipalContext, "Server Administrators");
                        break;
                    case "wa":
                        currentSearchGroup = GroupPrincipal.FindByIdentity(activeDirectoryPrincipalContext, "Workstation Administrators");
                        break;
                    default:
                        currentSearchGroup = GroupPrincipal.FindByIdentity(activeDirectoryPrincipalContext, "CANES USA Users");
                        break;
                }
                foreach (Principal p in currentSearchGroup.GetMembers()) {
                    if (adCred.Equals(p.Name)) {
                        //Console.WriteLine("Found User: " + p.Name + " in Group: "+ currentSearchGroup.Name);
                        return true;
                    }
                }
            } else { 
                throw new NullReferenceException(); 
            }
            return false;
        }
    }
}
