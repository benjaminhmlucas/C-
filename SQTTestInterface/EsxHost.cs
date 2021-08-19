using System;
using System.Collections;
using System.Collections.Generic;
using VMware.Vim;

namespace SQTTestInterface {

	public class EsxHost {
		public string ipAddress { get; set; }
		public bool userIsConnected { get; set; } = false;
		public bool userIsLoggedIn { get; set; } = false;
		public VimClient vimClient { get; }
		public EsxHost(string ipAddressIn) {
			ipAddress = ipAddressIn;
			vimClient = new VimClientImpl();
			vimClient.IgnoreServerCertificateErrors = true;
		}
		public bool ConnectToESXClient() {
			try {
				if (!userIsConnected) {
					vimClient.Connect("https://" + ipAddress + "/sdk");
					userIsConnected = true;
					Console.WriteLine("Connection to: \'" + ipAddress + "\' has been established.");
				} else {
					Console.WriteLine("Connection to: \'" + ipAddress + "\' has already been established.");
				}
			} catch (Exception e) {
				switch (e.GetType().ToString()) {
					//wrong IP Address
					case "VMware.Vim.VimEndpointNotFoundException":
						Console.WriteLine("IP Address: \'" + ipAddress + "\' is not an ESX host or is powered off!");
						userIsConnected = false;
						userIsLoggedIn = false;
						break;
					//wrong user name/password
					case "VMware.Vim.VimException":
						Console.WriteLine("VMware.Vim.VimException EXCEPTION CAUGHT!");
						userIsLoggedIn = false;
						break;
					default:
						Console.WriteLine("SPECIFIC EXCEPTION NOT CAUGHT!");
						break;
				}

			}
			return userIsConnected;
		}
		public bool TestLogonToESXClient(string testUser, string defaultTestPwd, ArrayList failedLoginList) {
			try {
				vimClient.Connect("https://" + ipAddress + "/sdk");
				userIsConnected = true;
				vimClient.Login(testUser, defaultTestPwd);
				List<EntityViewBase> list = new List<EntityViewBase>();
				list = vimClient.FindEntityViews(typeof(VirtualMachine), null, null, null);
				Console.WriteLine("User: \'" + testUser + "\' on: \'" + ipAddress + "\' isValid: True");
				userIsLoggedIn = true;
			} catch (Exception e) {
				switch (e.GetType().ToString()) {
					//wrong IP Address
					case "VMware.Vim.VimEndpointNotFoundException":
						Console.WriteLine("IP Address: \'" + ipAddress + "\' is not an ESX host or is powered off!");
						Console.WriteLine("Error: " + e.Message);
						if (!failedLoginList.Contains("IP Address: \'" + ipAddress + "\' is not an ESX host or is powered off!")) {
							failedLoginList.Add("IP Address: \'" + ipAddress + "\' is not an ESX host or is powered off!");
						}
						userIsConnected = false;
						userIsLoggedIn = false;
						break;
					//wrong user name/password
					case "VMware.Vim.VimException":
						Console.WriteLine("VMware.Vim.VimException EXCEPTION CAUGHT!");
						Console.WriteLine("Error: " + e.Message);
						failedLoginList.Add("User: \'" + testUser + "\' on: \'" + ipAddress + "\'");
						userIsLoggedIn = false;
						break;
					default:
						Console.WriteLine("SPECIFIC EXCEPTION NOT CAUGHT!");
						Console.WriteLine("Error: " + e.Message);
						break;
				}
			}
			return userIsLoggedIn;
		}
		public bool LogonToESXClient(string testUser, string defaultTestPwd) {
			try {
				vimClient.Connect("https://" + ipAddress + "/sdk");
				userIsConnected = true;
				vimClient.Login(testUser, defaultTestPwd);
				List<EntityViewBase> list = new List<EntityViewBase>();
				list = vimClient.FindEntityViews(typeof(VirtualMachine), null, null, null);
				Console.WriteLine("User: \'" + testUser + "\' on: \'" + ipAddress + "\' isValid: True");
				userIsLoggedIn = true;
			} catch (Exception e) {
				switch (e.GetType().ToString()) {
					//wrong IP Address
					case "VMware.Vim.VimEndpointNotFoundException":
						Console.WriteLine("IP Address: \'" + ipAddress + "\' is not an ESX host or is powered off!");
						Console.WriteLine("Error: " + e.Message);
						userIsConnected = false;
						userIsLoggedIn = false;
						break;
					//wrong user name/password
					case "VMware.Vim.VimException":
						Console.WriteLine("VMware.Vim.VimException EXCEPTION CAUGHT!");
						Console.WriteLine("Error: " + e.Message);
						userIsLoggedIn = false;
						break;
					default:
						Console.WriteLine("SPECIFIC EXCEPTION NOT CAUGHT!");
						Console.WriteLine("Error: " + e.Message);
						break;
				}
			}
			return userIsLoggedIn;
		}

	}
}