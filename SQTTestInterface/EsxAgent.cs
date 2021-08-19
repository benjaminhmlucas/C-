using System;
using System.Collections;
using System.Collections.Generic;
using VMware.Vim;

namespace SQTTestInterface {
    public class EsxAgent {
        private string user;
        private ArrayList esxIps;
        private ArrayList esxHosts = new ArrayList();
        public EsxAgent(string user, ArrayList esxIpsIn) {
            this.user = user;
            esxIps = esxIpsIn;

            foreach (string esxIp in esxIps) {
                esxHosts.Add(new EsxHost(esxIp));
            }
        }
        public bool MakeConnections(string pwd) {
            bool atLeastOneHostConnected = false;
            bool atLeastOneLoginSuccessful = false;
            foreach (EsxHost host in esxHosts) {
                if (host.ConnectToESXClient()) {
                    atLeastOneHostConnected = true;
                    if (host.LogonToESXClient(user, pwd)) {
                        atLeastOneLoginSuccessful = true;
                    }
                }
            }
            if (!atLeastOneHostConnected) {
                Console.Write("None of those hosts are Are ESX Servers or are turned off.  Please check your Ips.");
            } else {
                if (!atLeastOneLoginSuccessful) {
                    Console.Write("I was not able to login to any ESX hosts with the provided credentials.  Please check ESX user name and default test password.");
                    return false;
                }
            }
            return true;
        }
        public bool TestLogonToESXHost(ArrayList failedLoginList, string testUser, string defaultTestPwd) {
            bool atLeastOneSuccessfulLogin = false;
            if (esxHosts.Count > 0) {
                foreach (EsxHost esxHost in esxHosts) {
                    if (esxHost.TestLogonToESXClient(testUser, defaultTestPwd, failedLoginList)) {
                        atLeastOneSuccessfulLogin = true;
                    }
                }
            } else {
                Console.WriteLine("I don't have any ESX Server IPs To Check!!");
                failedLoginList.Add("There were no valid ESX Host IPs to test user: "+ testUser + "!");
                return atLeastOneSuccessfulLogin;
            }
            if (!atLeastOneSuccessfulLogin) {
                Console.Write("I was not able to login to any of the ESX IPs with the given ESX credentials.");
            }
            return atLeastOneSuccessfulLogin;
        }
        public ArrayList GetListOfESXVMs(string defaultTestPwd) {
            ArrayList vmList = new ArrayList();
            foreach (EsxHost esxHost in esxHosts) {                
                try {
                    esxHost.ConnectToESXClient();
                    esxHost.LogonToESXClient(user, defaultTestPwd);
                    List<EntityViewBase> list = new List<EntityViewBase>();
                    list = esxHost.vimClient.FindEntityViews(typeof(VirtualMachine), null, null, null);
                    Console.WriteLine("User: \'" + user + "\' on: \'" + esxHost.ipAddress + "\' isValid: True");
                    foreach (EntityViewBase item in list) {
                        VirtualMachine thisVM = (VirtualMachine)item;
                        Console.WriteLine("Adding EVB Item: " + thisVM.Name);
                        vmList.Add(thisVM);
                    }
                } catch (Exception e) {
                    switch (e.GetType().ToString()) {
                        //wrong IP Address
                        case "VMware.Vim.VimEndpointNotFoundException":
                            Console.WriteLine("IP Address: \'" + esxHost.ipAddress + "\' is not an ESX host or is powered off!");
                            break;
                        //wrong user name/password
                        case "VMware.Vim.VimException":
                            Console.WriteLine("VMware.Vim.VimException EXCEPTION CAUGHT!");
                            break;
                        default:
                            Console.WriteLine("SPECIFIC EXCEPTION NOT CAUGHT!");
                            break;
                    }
                    Console.WriteLine("It done Broke!");
                }
            }
            return vmList;
        }
    }
}
