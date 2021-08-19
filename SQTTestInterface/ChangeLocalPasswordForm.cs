using SQTTestInterface.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SQTTestInterface {
    public partial class ChangeLocalPasswordForm : Form {

        private Dictionary<string,TextBox> textBoxDictionary = new Dictionary<string, TextBox>();
        private static string noConfigString = StringResources.noConfigString;
        private static string passwordValidatorWindowTitle = StringResources.passwordValidatorWindowTitle;
        private static string configurationSaverWindowTitle = StringResources.configurationManagerMessageWindowTitle;
        private string configurationFilePath = StringResources.configurationFilePath;
        private string enteredOldPassword = noConfigString;
        private string enteredOldPassHash = noConfigString;
        private string savedOldPasswordHash = noConfigString;
        private string newPass1 = noConfigString;
        private string newPass2 = noConfigString;
        private string newPassHash = noConfigString;
        private string valueToReplace = "";

        public ChangeLocalPasswordForm() {
            InitializeComponent();
            CheckSettingsFiles();
        }
        private void CheckSettingsFiles() {
            if (!File.Exists(configurationFilePath)) {
                File.Create(configurationFilePath).Close();
            }
            StreamReader configFileReader = new StreamReader(configurationFilePath);
            configurationFilePath = Path.GetFullPath(configurationFilePath);
            Console.WriteLine("\nConfig File Location: " + configurationFilePath);
            string line;
            while ((line = configFileReader.ReadLine()) != null) {
                if (line.StartsWith("[LOCPH]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    savedOldPasswordHash = line.Replace("[LOCPH]", "");
                    continue;
                }
            }
            configFileReader.Close();
            textBoxDictionary.Clear();
            textBoxDictionary.Add("oldPassTextBox", oldPassTextBox);
            textBoxDictionary.Add("newPass1TextBox", newPass1TextBox);
            textBoxDictionary.Add("newPass2TextBox", newPass2TextBox);
        }
        //Event Handlers
        private void SubmitPasswordChange(object sender, EventArgs e) {
            //Get all TextBox Values            
            enteredOldPassword = textBoxDictionary["oldPassTextBox"].Text.Trim();
            enteredOldPassHash = SQTSupportFunctions.SupportFunctions.SHA256(enteredOldPassword); //Get hash for old password entered by user
            newPass1 = textBoxDictionary["newPass1TextBox"].Text.Trim();
            newPass2 = textBoxDictionary["newPass2TextBox"].Text.Trim();
            //Compare the values from the two new password Textboxes, ensure they match
            if (enteredOldPassword.Equals("")) {
                MessageBox.Show(new Form { TopMost = true },"Enter your current password!", passwordValidatorWindowTitle);
                return;
            }
            if (newPass1.Equals("") || newPass2.Equals("")) {
                MessageBox.Show(new Form { TopMost = true },"You need to enter you new password twice!", passwordValidatorWindowTitle);
                return;
            }
            if (enteredOldPassword.Equals("")) {
                MessageBox.Show(new Form { TopMost = true },"Enter your current password!", passwordValidatorWindowTitle);
                return;
            }
            if (!savedOldPasswordHash.Equals(enteredOldPassHash)) {
                MessageBox.Show(new Form { TopMost = true },"Your old password entry wasn't correct!", passwordValidatorWindowTitle);
                return;
            }
            if (!newPass1.Equals(newPass2)) {
                MessageBox.Show(new Form { TopMost = true },"Your new password entries don't match!", passwordValidatorWindowTitle);
                return;
            }
            //Record new hashed value in the config file
            if (!File.Exists(configurationFilePath)) {
                File.Create(configurationFilePath).Close();
            }
            //Read file and get specific line to replace
            StreamReader configFileReader = new StreamReader(configurationFilePath);
            configurationFilePath = Path.GetFullPath(configurationFilePath);
            Console.WriteLine("\nConfig File Location: " + configurationFilePath);

            string line;//holds read line to be read, identifies line by [xxx] value, then passes to appropriate variable for use later
            while ((line = configFileReader.ReadLine()) != null) {
                if (line.StartsWith("[LOCPH]")) {
                    Console.WriteLine("Getting info from Text Input... " + line);
                    valueToReplace = line;
                }
            }
            configFileReader.Close();
            //Replace old config line and set text file contents                    
            String configFileText = File.ReadAllText(configurationFilePath);
            newPassHash = SQTSupportFunctions.SupportFunctions.SHA256(newPass1); //Get hash for old password entered by user
            configFileText = configFileText.Replace(valueToReplace, "[LOCPH]" + newPassHash);
            try {
                File.WriteAllText(configurationFilePath, configFileText);
                Console.WriteLine("Value Saved: " + "[LOCPH]" + newPassHash);
            } catch (Exception e2) {
                MessageBox.Show(new Form { TopMost = true },"There was an error trying to write to your config file!\nPlease check the file isn't being used by another program and retry save action.\n" + e2.Message, configurationSaverWindowTitle);
            }
            Close();
        }
        private void CancelPasswordChange(object sender, EventArgs e) {
            this.Close();
        }



    }
}
