using SQTSupportFunctions;
using SQTTestInterface.Resources;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SQTTestInterface {
    public partial class LoginForm : Form {

        private Hashtable textBoxHT = new Hashtable();
        //Message Strings
        private static string loginValidatorMessageWindowTitle = StringResources.loginValidatorMessageWindowTitle;
        private static string configurationManagerMessageWindowTitle = StringResources.configurationManagerMessageWindowTitle;
        //config file 
        private string configurationFilePath = StringResources.configurationFilePath;
        private string valueToSave = "ERROR!";
        private string valueToReplace = "ERROR!";
        private ArrayList errorList = new ArrayList();
        //user variables
        private string hullNumber = StringResources.dftHull;
        private string adIP = StringResources.dftADIP;
        private ArrayList adIPList = new ArrayList(); //used to create ADAgent
        private string user = StringResources.dftDA;
        private string hardPass = "<PASSWORDHERE>";//REMOVE LINE AT FINISH - Used to bypass password entry for testing
        private string pwd = StringResources.dftPH;
        private string localUser = StringResources.dftUser;
        private string localPwdHash = StringResources.dftPH;//This is compared to user input after it has been hashed
        private string savedPwd = "";//REMOVE LINE AT FINISH - Used to bypass password entry for testing
        //loaded if configuration file isn't found
        private string defaultSettings = StringResources.defaultSettings;
        //Active Directory Agent
        private ADAgent aDAgent;
        public LoginForm() {
            //set Window Position
            this.CenterToScreen();
            InitializeComponent();
            CheckSettingsFiles();
            if (hullNumber.Equals("<HULL#>")) { //if config file was missing and had to be set to template, check connect locally box
                connectLocalCheckBox.Checked = true;
            } else {
                SQTSupportFunctions.SupportFunctions.CleanConfig(configurationFilePath);
            }
        }
        //Event Handlers
        private void LoginForm_Load(object sender, EventArgs e) {

        }
        private void ConnectLocalCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (connectLocalCheckBox.Checked) {
                adIPTextBox.Enabled = false;
                adIPLabel.Enabled = false;
                userNameTextBox.Text = localUser;
                pwdLoginTextBox.Text = savedPwd;//REMOVE LINE AT FINISH - Used to bypass password entry for testing
            } else {
                adIPTextBox.Enabled = true;
                adIPLabel.Enabled = true;
                userNameTextBox.Text = user;
                pwdLoginTextBox.Text = hardPass;//REMOVE LINE AT FINISH - Used to bypass password entry for testing

            }
        }
        private void DefaultConfigBtn_Click(object sender, EventArgs e) {
            DialogResult resetToDefaultResult = MessageBox.Show(new Form { TopMost = true },"This will completely erase all settings!", "Are you sure?", MessageBoxButtons.OKCancel);
            if (resetToDefaultResult == DialogResult.OK) {
                if (!File.Exists(configurationFilePath)) {
                    File.Create(configurationFilePath).Close();
                }
                File.WriteAllText(configurationFilePath, defaultSettings);
                CheckSettingsFiles();
            }
        }
        private void LoginCancel_Clicked(object sender, EventArgs e) {
            Environment.Exit(0);
        }
        private void LoginConnect_Clicked(object sender, EventArgs e) {
        
            user = userNameTextBox.Text;
            pwd = pwdLoginTextBox.Text;
         
            //start login
            if (ValidateInput()){
                //Hide main window
                Hide();
                
                //use AD Agent is Connect Local isn't checked
                if (!connectLocalCheckBox.Checked) {
                    if (CheckWindowsLoginCredentials()) {
                        //After verifying AD Credentials attempt to connect to the ESX host with data from SSAManager.conf file and open the Main Interface
                        SupportFunctions.ShowLoadingPopUp("Loading ESX...", Properties.Resources.robot2);
                        System.Threading.Thread.Sleep(1000);//let pop-up thread catch up
                        MainInterface newWindow = new MainInterface(pwd, aDAgent);
                        Console.WriteLine("Login Success!");
                        newWindow.Show();
                    } else {
                        Show();
                    }
                    SupportFunctions.CloseLoadingPopUp();
                } else {
                    SupportFunctions.CleanConfig(configurationFilePath);
                    Console.WriteLine("Before Comparison: " + SupportFunctions.SHA256(pwd));
                    if (user.Equals(localUser) && SupportFunctions.SHA256(pwd).Equals(localPwdHash)) {
                        pwd = "";//this allows program to know it needs to tell the user to set the default test password
                        SupportFunctions.ShowLoadingPopUp("Loading ESX...", Properties.Resources.robot2);
                        System.Threading.Thread.Sleep(1000);//let pop-up thread catch up
                        MainInterface newWindow = new MainInterface(pwd, aDAgent);
                        Console.WriteLine("Login Success!");
                        newWindow.Show();
                    } else {
                        MessageBox.Show(new Form { TopMost = true },"Your local password or username is incorrect!\nPlease Re-Enter the credentials.", loginValidatorMessageWindowTitle);
                        Show();
                    }
                    SupportFunctions.CloseLoadingPopUp();
                }
            }
        }
        //HELPER METHODS
        private bool ValidateInput() {
        errorList.Clear();
            bool inputIsValid = true;
                if ((user.Equals("") || pwd.Equals(""))) {
                MessageBox.Show(new Form { TopMost = true },"Please enter a User Name and Password!", loginValidatorMessageWindowTitle);
                inputIsValid = false;
                return inputIsValid;
            }
                
            if (!connectLocalCheckBox.Checked) {
                adIP = adIPTextBox.Text;
                foreach (DictionaryEntry pair in textBoxHT) {
                    TextBox currentTB = (TextBox)textBoxHT[pair.Key];
                    switch (pair.Key) {
                        case "adIPTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.ValidateIp(currentTB.Text))) {
                                errorList.Add("Ship AD: Value entered (" + currentTB.Text + ") is not an IP!");
                            }
                            break;
                        case "userNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text))) {
                                errorList.Add("Domain Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods");
                            } else {
                                valueToSave = ("[USER][DA]" + currentTB.Text);
                            }
                            break;
                        default:
                            errorList.Add("There was an issue finding the " + pair.Key + " Text Box!");
                            break;
                    }
                    //Captured Value Checker
                    //Console.WriteLine("Captured Key: " + pair.Key + " -->>>Captured Value: " + currentTB.Text);
                }
                if (errorList.Count > 0) {
                    StringBuilder errorListSB = new StringBuilder();
                    foreach (string errorDescription in errorList) {
                        errorListSB.Append(">" + errorDescription + "!\n");
                    }
                    MessageBox.Show(new Form { TopMost = true },"I need you to fix these issues:\n" + errorListSB.ToString(), loginValidatorMessageWindowTitle);
                    inputIsValid = false;
                }                               
            }
            return inputIsValid;
        }
        private bool CheckWindowsLoginCredentials() {
            //Show loading AD Message
            SupportFunctions.ShowLoadingPopUp("Loading AD...", Properties.Resources.robot1);
            System.Threading.Thread.Sleep(1000);//let pop-up thread catch up
            if (VerifyLoginCredential(pwd, adIP, user)) {
                //Check for save file
                if (!File.Exists(configurationFilePath)) {
                    File.Create(configurationFilePath).Close();
                }
                //Read file and get specific line to replace
                StreamReader configFileReader = new StreamReader(configurationFilePath);
                configurationFilePath = Path.GetFullPath(configurationFilePath);
                Console.WriteLine("\nConfig File Location: " + configurationFilePath);

                string line;//holds read line to be read, identifies line by [xxx] value, then passes to appropriate variable for use later

                while ((line = configFileReader.ReadLine()) != null) {
                    if (line.StartsWith("[USER][DA]")) {
                        Console.WriteLine("Getting info from Text Input... " + line);
                        valueToReplace = line;
                    }
                }
                configFileReader.Close();

                //Replace old config line and set text file contents                    
                String configFileText = File.ReadAllText(configurationFilePath);
                if (!valueToReplace.Equals("")) {
                    configFileText = configFileText.Replace(valueToReplace, valueToSave);
                    //Console.WriteLine("Removing value: " + valuesToReplace[i].ToString());
                } else {
                    MessageBox.Show(new Form { TopMost = true },"I can't find your config file value!  I am now adding it!", loginValidatorMessageWindowTitle);
                    configFileText += (valueToSave);
                }
                //Console.WriteLine("Removing value: " + valuesToReplace[i].ToString());
                try {
                    File.WriteAllText(configurationFilePath, configFileText);
                    Console.WriteLine("Value Saved: " + valueToSave);
                } catch (Exception e2) {
                    MessageBox.Show(new Form { TopMost = true },"There was an error trying to write to your config file!\nPlease check the file isn't being used by another program and retry save action.\n" + e2.Message, configurationManagerMessageWindowTitle);
                }
                //once complete, close loading pop up
                SupportFunctions.CloseLoadingPopUp();
                return true;
            } else {
                return false;
            }
        }
        private bool VerifyLoginCredential(string word, string adIP, string adCred) {
            aDAgent = new ADAgent(adIPList, adCred, word);
            bool connected = false;
            try {
                connected = aDAgent.VerifyLoginCredentialSingleSever(adIP, adCred, word, "da");
            } catch (NullReferenceException e) {
                MessageBox.Show(new Form { TopMost = true }, "I am having trouble connecting to your AD on IP Address: " + adIP+". Check your IP Address!",
                    StringResources.loginValidatorMessageWindowTitle);
            } catch {
                MessageBox.Show(new Form { TopMost = true }, "I am having trouble connecting to your AD on IP Address: " + adIP + ". Check your username and password!",
                    StringResources.loginValidatorMessageWindowTitle);
            }
            return connected;
        }
        private void CheckSettingsFiles() {
            if (!File.Exists(configurationFilePath)) {
                MessageBox.Show(new Form { TopMost = true },"I can't find the config file named: " + configurationFilePath.Replace(".\\", "") +
                    "You will need to login with the default local admin account and reconfigure your settings.  If you " +
                    "still have the configuration file, you can place it in the same folder as this executable to reload your" +
                    "settings!  You will have to overwrite the file I am about to create, make sure the new config file is called:" +
                    configurationFilePath.Replace(".\\", ""), configurationManagerMessageWindowTitle);
                File.Create(configurationFilePath).Close();
                File.WriteAllText(configurationFilePath, defaultSettings);
                CheckSettingsFiles();
                return;
            }
            StreamReader configFileReader = new StreamReader(configurationFilePath);
            configurationFilePath = Path.GetFullPath(configurationFilePath);
            Console.WriteLine("\nConfig File Location: " + configurationFilePath);
            string line;
            while ((line = configFileReader.ReadLine()) != null) {
                if (line.StartsWith("[HULL]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    hullNumber = line.Replace("[HULL]", "");
                    continue;
                }
                if (line.StartsWith("[ADIP]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    line = line.Replace("[ADIP]", "");
                    adIPList = new ArrayList(line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                    adIP = (string)adIPList[0];
                    continue;
                }
                if (line.StartsWith("[USER][DA]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    user = line.Replace("[USER][DA]", "");
                    user = user.Replace(hullNumber + "\\", "");
                    continue;
                }
                if (line.StartsWith("[USER][LOC]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    localUser = line.Replace("[USER][LOC]", "");
                    continue;
                }
                if (line.StartsWith("[LOCPH]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    localPwdHash = line.Replace("[LOCPH]", "");
                    continue;
                }

            }
            configFileReader.Close();
            adIPTextBox.Text = adIP;
            pwdLoginTextBox.Text = hardPass;//REMOVE LINE AT FINISH - Used to bypass password entry for testing
            textBoxHT.Clear();
            if (connectLocalCheckBox.Checked) {
                userNameTextBox.Text = localUser;
            } else {
                if (user.Equals(StringResources.dftDA)) {
                    user = user.Replace("<DOMAIN>\\", "");
                }
                userNameTextBox.Text = user;
            }
            textBoxHT.Add("userNameTextBox", userNameTextBox);
            textBoxHT.Add("adIPTextBox", adIPTextBox);
        }
    }    
}
