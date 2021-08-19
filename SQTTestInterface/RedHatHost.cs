using System;
using System.Collections;
using System.Collections.Generic;
using VMware.Vim;

namespace SQTTestInterface {

	public class RedHatHost {
		public string ipAddress { get; set; }
		public bool userIsConnected { get; set; } = false;
		public bool userIsLoggedIn { get; set; } = false;
		public RedHatHost(string ipAddressIn) {
			ipAddress = ipAddressIn;
		}
		public bool ConnectToESXClient(VimClient vimClient) {
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
						break;
					//wrong user name/password
					case "VMware.Vim.VimException":
						Console.WriteLine("VMware.Vim.VimException EXCEPTION CAUGHT!");
						break;
					default:
						Console.WriteLine("SPECIFIC EXCEPTION NOT CAUGHT!");
						break;
				}

			}
			return userIsConnected;
		}
		public bool LogonToESXClient(string user, string pwd, VimClient vimClient) {
			try {
				if (!userIsLoggedIn) {
					vimClient.Login(user, pwd);
					userIsLoggedIn = true;
					Console.WriteLine("User: \'" + user + "\' has logged onto: \'" + ipAddress + "\'");
				} else {
					Console.WriteLine("User: \'" + user + "\' was already logged onto: \'" + ipAddress + "\'");
				}
			} catch (Exception e) {
				switch (e.GetType().ToString()) {
					//wrong IP Address
					case "VMware.Vim.VimEndpointNotFoundException":
						Console.WriteLine("IP Address: \'" + ipAddress + "\' is not an ESX host or is powered off!");
						break;
					//wrong user name/password
					case "VMware.Vim.VimException":
						Console.WriteLine("VMware.Vim.VimException EXCEPTION CAUGHT!");
						break;
					default:
						Console.WriteLine("SPECIFIC EXCEPTION NOT CAUGHT!");
						break;
				}
			}
			return userIsLoggedIn;
		}
	}
}