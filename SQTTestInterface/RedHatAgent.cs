using System;
using System.Collections;
using System.Collections.Generic;
using VMware.Vim;

namespace SQTTestInterface {
    public class RedHatAgent {
        private string pwd;
        private string user;
        private ArrayList esxIps;
        private ArrayList esxHosts = new ArrayList();
        private VimClient vimClient { get; }
        public RedHatAgent(string pwd, string user, ArrayList esxIpsIn) {

            this.pwd = pwd;
            this.user = user;
            esxIps = esxIpsIn;

            vimClient = new VimClientImpl();
            vimClient.IgnoreServerCertificateErrors = true;
            foreach (string esxIp in esxIps) {
                esxHosts.Add(new RedHatHost(esxIp));
            }
        }
        public bool MakeConnections() {
            foreach (RedHatHost host in esxHosts) {
                try {
                    host.ConnectToESXClient(vimClient);
                    host.LogonToESXClient(user, pwd, vimClient);
                } catch (Exception e) {
                    switch (e.GetType().ToString()) {
                        //wrong IP Address
                        case "VMware.Vim.VimEndpointNotFoundException":
                            Console.WriteLine("IP Address: \'" + host.ipAddress + "\' is not an ESX host or is powered off!");
                            break;
                        //wrong user name/password
                        case "VMware.Vim.VimException":
                            Console.WriteLine("VMware.Vim.VimException EXCEPTION CAUGHT!");
                            break;
                        default:
                            Console.WriteLine("SPECIFIC EXCEPTION NOT CAUGHT!");
                            break;
                    }
                    return false;
                }
            }
            return true;
        }
        public ArrayList GetListOfESXVMs() {
            ArrayList vmList = new ArrayList();
            foreach (string ip in esxIps) {                
                vimClient.IgnoreServerCertificateErrors = true;
                try {
                    vimClient.Connect("https://" + ip + "/sdk");
                    vimClient.Login(user, pwd);
                    List<EntityViewBase> list = new List<EntityViewBase>();
                    list = vimClient.FindEntityViews(typeof(VirtualMachine), null, null, null);
                    Console.WriteLine("User: \'" + user + "\' on: \'" + ip + "\' isValid: True");
                    foreach (EntityViewBase item in list) {
                        VirtualMachine thisVM = (VirtualMachine)item;
                        Console.WriteLine("Adding EVB Item: " + thisVM.Name);
                        vmList.Add(thisVM);
                    }
                } catch (Exception e) {
                    switch (e.GetType().ToString()) {
                        //wrong IP Address
                        case "VMware.Vim.VimEndpointNotFoundException":
                            Console.WriteLine("IP Address: \'" + ip + "\' is not an ESX host or is powered off!");
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
