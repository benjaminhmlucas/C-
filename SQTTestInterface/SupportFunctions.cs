using Renci.SshNet;
using SQTTestInterface;
using SQTTestInterface.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SQTSupportFunctions {
    class SupportFunctions {
        //Loading Components Window
        private static ComponentsLoadingWindow componentsLoadingWindow { get; set; }
        public static void ShowLoadingPopUp(string loadingMessage, Bitmap loadingAnimationName) {
            new Thread(() => {  
                componentsLoadingWindow = ComponentsLoadingWindow.ShowSplashScreen(loadingMessage, loadingAnimationName);
            }).Start();
        }
        public static void CloseLoadingPopUp() {
            ComponentsLoadingWindow.CloseForm();
        }
        public static bool ValidateIp(string ip) {
            bool isIpAddressFormat = true;
            String errorMessage = "There was an issue with this ip: ";
            ArrayList octetList = new ArrayList();
            octetList.AddRange(ip.Split('.'));
            if (octetList.Count != 4) {
                Console.WriteLine("This Ip has incorrect number of octets: " + ip);
                isIpAddressFormat = false;
                return isIpAddressFormat;
            }
            foreach (String ipOctet in octetList) {
                try {
                    int doesItCastToIntTestVariable = Int32.Parse(ipOctet);
                    if (doesItCastToIntTestVariable > 255 || doesItCastToIntTestVariable < 0) {
                        throw new Exception("One octet is less than 0 or greater than 255: ");
                    }
                } catch (Exception e) {
                    errorMessage = (e.Message).Replace(".", ": ");
                    isIpAddressFormat = false;
                    break;
                }
            }
            if (!isIpAddressFormat) {
                Console.WriteLine(errorMessage + ip); 
            } else {
                //tells user that ip is good->this is unnecessary for current uses
                //Console.WriteLine("This Ip is formatted correctly: " + ip);
            }
            return isIpAddressFormat;
        }
        //Settings and configuration checks
        public static void CleanConfig(string configurationFilePath) {
            ArrayList valuesToSaveArrayList = new ArrayList();
            ArrayList settingsPrefixList = new ArrayList( StringResources.configSettingsLineList.Split(','));
            Dictionary<string,bool> settingsPresentDictionary = new Dictionary<string, bool>();
            foreach (string prefix in settingsPrefixList) {
                settingsPresentDictionary.Add(prefix,false);
            }
            //Check for config file
            if (!File.Exists(configurationFilePath)) {
                File.Create(configurationFilePath).Close();
            }
            //Read file and get specific line to replace
            StreamReader configFileReader = new StreamReader(configurationFilePath);
            configurationFilePath = Path.GetFullPath(configurationFilePath);
            Console.WriteLine("\nConfig File Location: " + configurationFilePath);

            string line;//holds read line to be read, identifies line by [xxx] value, then passes to appropriate variable for use later

            //Read file to get values values that aren't changed on this screen from config file
            while ((line = configFileReader.ReadLine()) != null) {
                //Save values that can't be modified here
                foreach (string settingsLinePrefix in settingsPrefixList) {
                    if (line.StartsWith(settingsLinePrefix)) { 
                        valuesToSaveArrayList.Add(line);
                        settingsPresentDictionary[settingsLinePrefix] = true;
                    }
                }
            }

            foreach (string settingsLinePrefix in settingsPrefixList) {
                if (!settingsPresentDictionary[settingsLinePrefix]) {
                    switch (settingsLinePrefix) {
                        case "[ADIP]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftADIP);
                            break;
                        case "[ESXIP]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftESXIP);
                            break;
                        case "[REDHAT]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftRH);
                            break;
                        case "[HULL]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftHull);
                            break;
                        case "[LOCPH]":
                            MessageBox.Show("Local username file has been corrupted or is missing data.  The local username will be set back to default for security reasons.", "Local User File Corrupted!", MessageBoxButtons.OK);
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftPH);
                            break;
                        case "[USER][LOC]":
                            MessageBox.Show("Local user password has been corrupted or is missing data.  The local password will be set back to default for security reasons.", "Local User File Corrupted!", MessageBoxButtons.OK);
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftUser);
                            break;
                        case "[USER][DA]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftDA);
                            break;
                        case "[USER][SA]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftSA);
                            break;
                        case "[USER][WA]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftWA);
                            break;
                        case "[USER][REG]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftReg);
                            break;
                        case "[USER][ESX]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftESX);
                            break;
                        case "[USER][SM]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftSSH);
                            break;
                        case "[USER][SSH]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftSM);
                            break;
                        case "[WEBTEST][EXTERNAL]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftWebExt);
                            break;
                        case "[WEBTEST][INTERNAL]":
                            valuesToSaveArrayList.Add(settingsLinePrefix + StringResources.dftWebInt);
                            break;
                    };
                }
            }
            configFileReader.Close();

            //Set text file contents
            valuesToSaveArrayList.Sort();
            StringBuilder configFileTextBuilder = new StringBuilder();
            for (int i = 0; i < valuesToSaveArrayList.Count; i++) {
                configFileTextBuilder.Append(valuesToSaveArrayList[i].ToString() + "\n");
                Console.WriteLine("Value To Saved: " + valuesToSaveArrayList[i].ToString());
            }
            try {
                File.WriteAllText(configurationFilePath, configFileTextBuilder.ToString().Trim());
            } catch (Exception e) {
                MessageBox.Show(new Form { TopMost = true },"There was an error trying to write to your config file!\nPlease check the file isn't being used by another program and retry save action.\n" + e.Message, "IP Address Validator");
            }
        }        
        public static bool CheckSettingsLinePrefixAndAppendToList(ArrayList valuesToSave, string lineToCheck,string prefixToCheckFor) {
            bool settingFound = false;
            if (lineToCheck.StartsWith(prefixToCheckFor)) {
                valuesToSave.Add(lineToCheck);
                settingFound = true;
            };
            if (settingFound == false) {
                if (!valuesToSave.Contains(prefixToCheckFor)) {
                    valuesToSave.Add(prefixToCheckFor);
                } 
            };
            return settingFound;
        }
        public static bool TestUserNameAndDomainRegex(string stringToTest) {
            //Matches what canes admin users names should be/Windows Rules
            if (stringToTest.Contains('\\')) {
                stringToTest = stringToTest.Remove(0, (stringToTest.IndexOf('\\') + 1));
            }
            Regex rx = new Regex(@"^[a-zA-Z0-9\-\._]{1,61}$");
            //Console.WriteLine("String: " + stringToTest);
            //Console.WriteLine("Matched Pattern: " + rx.IsMatch(stringToTest));
            return rx.IsMatch(stringToTest);
        }
        public static bool TestESXUserNameAndDomainRegex(string stringToTest) {
            //Matches what canes admin users names should be/Windows Rules
            if (stringToTest.Contains('\\')) {
                stringToTest = stringToTest.Remove(0, (stringToTest.IndexOf('\\') + 1));
            }
            Regex rx = new Regex(@"^[a-zA-Z0-9\-\._@]{1,61}$");
            //Console.WriteLine("String: " + stringToTest);
            //Console.WriteLine("Matched Pattern: " + rx.IsMatch(stringToTest));
            return rx.IsMatch(stringToTest);
        }
        public static bool TestHullRegex(string stringToTest) {
            //Matches what canes hull names should be/No Special characters
            Regex rx = new Regex(@"^[a-zA-Z0-9]+$");
            //Console.WriteLine("String: " + stringToTest);
            //Console.WriteLine("Matched Pattern: " + rx.IsMatch(stringToTest)); 
            return rx.IsMatch(stringToTest);
        }
        public static bool TestWebsiteString(string text,Boolean showMessages) {
            string mixedRegex = StringResources.mixedRegex;
            string numberRegex = StringResources.numberRegex;
            ArrayList acceptableTopLevelDomains = new ArrayList(StringResources.acceptableTopLevelDomains.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            ArrayList urlStrings; //whole address
            ArrayList authorityString; //first section after https/http
            ArrayList endOfAuthorityString; //used to check if port is present
            ArrayList errorList = new ArrayList(); //list for accumulated errors
            bool matched;
            MatchCollection mc;

            if (text.Length < 1) {//check string length
                MessageBox.Show(new Form { TopMost = true },"No website Entered! Please enter a website!", StringResources.websiteManagerMessageWindowTitle);
                return false;
            }

            if (text.Length > 260) {//check string length
                MessageBox.Show(new Form { TopMost = true },"Too many characters! Please enter a website with less characters!", StringResources.websiteManagerMessageWindowTitle);
                return false;
            }

            mc = Regex.Matches(text, mixedRegex);
            matched = false;
            foreach (Match m in mc) {
                matched = true;
            }
            if (!matched) { //check string contents, the websites we check don't use many special characters so lets restrict that
                errorList.Add(">Check characters in URL:(Letters, numbers, hyphens, underscores, forward slashes, and periods allowed!/)>");
            }
            urlStrings = new ArrayList(text.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries));
            if (urlStrings.Count > 1) {
                if (!urlStrings[0].Equals("http:") && !urlStrings[0].Equals("https:")) {
                    //add error to list if beginnning string doesn't match "https:" or "http:"
                    errorList.Add(">Check protocol for URL:('https://' or 'http://' is allowed!)>");
                } else {
                    //if it matches remove evaluated portion and continue
                    text = text.Replace((urlStrings[0].ToString())+"//", "");
                }
            }
            urlStrings = new ArrayList(text.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            authorityString = new ArrayList(urlStrings[0].ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries));
            endOfAuthorityString = new ArrayList(authorityString[authorityString.Count-1].ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries));
            //check if a port was in the address
            if (endOfAuthorityString.Count < 2) { //port not present                        
                if (acceptableTopLevelDomains.Contains(authorityString[(authorityString.Count - 1)])) { // if the top level domain is in list, continue or add to error list
                } else { //add error to list if Top Level Domain doesn't match 'mil','org','gov', or 'com'
                    errorList.Add(">Check Top Level Domain for URL:('mil','org','gov', or 'com'  is allowed!)>");
                }                    
            } else { //port is present, check both pieces
                if (!acceptableTopLevelDomains.Contains(endOfAuthorityString[0])) { // if the top level domain is in list, continue or add to error list
                    errorList.Add(">Check Top Level Domain for URL:('mil','org','gov', or 'com'  is allowed!)>");
                }
                mc = Regex.Matches(endOfAuthorityString[1].ToString(), numberRegex);
                matched = false;
                foreach (Match m in mc) {
                    matched = true;
                }
                if (!matched) { // if port is numbers only, continue or add to server list
                    //add error to list if port isn't numbers only
                    errorList.Add(">Check port for URL:(Ports can only have numbers!)>");
                }
            }
            if (errorList.Count > 0) { //if there are errors, return false and give user a fail message
                StringBuilder errorListSB = new StringBuilder();
                errorListSB.Append("We have found errors in youe web address: " + text + ". Please correct the following errors:\n\n");
                foreach (string error in errorList) {
                    errorListSB.Append(error + "\n\n");
                }
                MessageBox.Show(new Form { TopMost = true },errorListSB.ToString(), StringResources.websiteManagerMessageWindowTitle);
                return false;
            } else { //return true and give user success message//unless this is for import validation/then message switch will be set to false/
                if (showMessages) {
                    MessageBox.Show(new Form { TopMost = true },"Your website checks out.  I will add it now.  Thanks for your courage.", StringResources.websiteManagerMessageWindowTitle);
                }
                return true;
            }
        }
        public static string RemoveHullFromUser(string userString) {
            if (userString.Contains("\\")) {
                userString = userString.Remove(0, (userString.IndexOf('\\')) + 1);
                return RemoveHullFromUser(userString);               
            } else {
                return userString;
            }
        }
        public static void TestLogonToLinuxSingle(SshClient client) {
            using (client) {
                try {
                    client.Connect();
                    SshCommand sc = client.CreateCommand("echo 'was granted access'");
                    sc.Execute();
                    string answer = sc.Result;
                    Console.WriteLine("Returned Value: " + client.ConnectionInfo.Username + " " + answer.Replace("\n", "") + " on " + client.ConnectionInfo.Host);
                } catch (Exception e) {
                    Console.WriteLine("Returned Value: " + client.ConnectionInfo.Username + " was NOT granted access on " + client.ConnectionInfo.Host + " with Error: " + e.Message);
                }
            }
        }
        public static string GenerateHashString(HashAlgorithm algo, string text) {
            // Compute hash from text parameter
            algo.ComputeHash(Encoding.UTF8.GetBytes(text));

            // Get has value in array of bytes
            var result = algo.Hash;

            // Return as hexadecimal string
            return string.Join(
                string.Empty,
                result.Select(x => x.ToString("x2")));
        }
        public static string SHA256(string text) {
            var result = default(string);

            using (var algo = new SHA256Managed()) {
                result = GenerateHashString(algo, text);
            }
            return result;
        }
        //site test functions
        public static void AddListViewItemAndRefresh(ListView listview, ArrayList list, string newItem) {
            try {
                list.Add(newItem);
                listview.BeginUpdate();
                listview.Items.Add(newItem);
                listview.Refresh();
            } finally {
                listview.EndUpdate();
            }
        }
        public static void removeListViewItemAndRefresh(ListView listview, ArrayList list, ListViewItem itemToRemove) {
            try {
                list.Remove(itemToRemove.Text);
                listview.BeginUpdate();
                listview.Items.Remove(itemToRemove);
                listview.Refresh();
            } finally {
                listview.EndUpdate();
                
            }
        }

    }
}       
