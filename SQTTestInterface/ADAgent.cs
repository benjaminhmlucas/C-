using System;
using System.Collections;
using System.Windows.Forms;
using SQTSupportFunctions;
using SQTTestInterface.Resources;

namespace SQTTestInterface {
    public class ADAgent {
        private string user;
        private ArrayList adIps = new ArrayList();
        public ArrayList adServers { get; } = new ArrayList();
        public ADAgent(ArrayList adIpsIn, string userIn, string pwd) {
            user = userIn;
            adIps = adIpsIn;
            ArrayList badADIps = new ArrayList();
            foreach (string ip in adIps) {
                if (SupportFunctions.ValidateIp(ip)) {
                    try {
                        adServers.Add(new ADServer(ip, user, pwd));
                    } catch (NullReferenceException e) {
                        Console.WriteLine("Error making AD Server Object with IP address: " + ip + ".\n Error: "+e.Message);
                        badADIps.Add(ip);
                    }                    
                } else {
                    Console.WriteLine("IP Address: "+ip+" is not an IP Address.  I will not make an AD Server with this address. Removing IP from list.");
                    badADIps.Add(ip);
                }
            }
            foreach (string ip in badADIps) {
                adIps.Remove(ip);
            }            
        }
        public bool VerifyLoginCredentialAllServers(string userToVerify,string testPwd, string userType) {
            bool connectionMade = false;
            if (adServers.Count > 0) {                
                foreach (ADServer adServer in adServers) {
                    connectionMade = VerifyLoginCredentialSingleSever(adServer.ipAddress, userToVerify, testPwd, userType);
                    if (connectionMade) { return connectionMade; }
                }
            } else {
                MessageBox.Show(new Form { TopMost = true },"The AD Agent doesn't have any valid AD server IP addresses! Skipping login credential check!",
                    StringResources.loginValidatorMessageWindowTitle);
            }
            return connectionMade;
        }
        public bool VerifyLoginCredentialSingleSever(string adIP,string userToVerify,string testPwd,string userType) {
            ADServer adServer = new ADServer(adIP, userToVerify, testPwd);
            return adServer.ValidateCredentials(userToVerify, testPwd, userType); ;
        }

        public void AddAdIpsAndServers(string ipIn, string adCred, string pwd) {
            if (SupportFunctions.ValidateIp(ipIn)) {
                adIps.Add(ipIn);
                adServers.Add(new ADServer(ipIn, adCred, pwd));
            } else {
                MessageBox.Show(new Form { TopMost = true },"This not a valid IP address IP Address: " + ipIn, StringResources.ipAddressValidatorMessageWindowTitle);
            }
        }
        public void RemoveAdIpsAndServers(ADServer AdServerToRemove) {
            try {
                adServers.Remove(AdServerToRemove);
                adIps.Remove(AdServerToRemove.ipAddress);
            } catch (Exception e) {
                MessageBox.Show(new Form { TopMost = true },"I cannot remove this IP Address or Servers: " + AdServerToRemove.ipAddress, StringResources.loginValidatorMessageWindowTitle);
            }
        }
        public void ClearAdIpsAndServers() {
            adIps.Clear();
            adServers.Clear();
        }
    }
}
