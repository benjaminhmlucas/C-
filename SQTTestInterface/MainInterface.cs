using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.Windows.Forms;
using Renci.SshNet;
using Microsoft.VisualBasic;
using SQTTestInterface.Resources;
using VMware.Vim;
using ListView = System.Windows.Forms.ListView;
using View = System.Windows.Forms.View;
using SQTSupportFunctions;

namespace SQTTestInterface {


    public partial class MainInterface : Form {

        private int externalSiteTestLines = 1;//number of external site test lines when program opens
        //Strings for texts and messages.
        private static string noConfigString = StringResources.noConfigString;
        private static string noADConfigString = StringResources.noADConfigString;
        private static string accountValidatorMessageWindowTitle = StringResources.accountValidatorMessageWindowTitle;
        private static string defaultPassMessageWindowTitle = StringResources.defaultPassMessageWindowTitle;
        private static string actionCanceledMessageWindowTitle = StringResources.actionCanceledMessageWindowTitle;
        private static string ipAddressValidatorMessageWindowTitle = StringResources.ipAddressValidatorMessageWindowTitle;
        private static string hullandUserValidatorMessageWindowTitle = StringResources.hullandUserValidatorMessageWindowTitle;
        private static string externalSiteTestManagerMessageWindowTitle = StringResources.externalSiteTestManagerMessageWindowTitle;
        private static string websiteManagerMessageWindowTitle = StringResources.websiteManagerMessageWindowTitle;
        //Configuration & Log Files
        private string configurationFilePath = StringResources.configurationFilePath;
        private string externalSiteTestLogFilePath = StringResources.externalSiteTestLogFilePath;
        //AccountVariables
        private string hullNumber = noConfigString;
        private string domain = noConfigString;
        private string regUserName = noConfigString;
        private string domainAdminName = noConfigString;
        private string serverAdminName = noConfigString;
        private string workstationAdminName = noConfigString;
        private string sshUserName = noConfigString;
        private string smUserName = noConfigString;
        private string esxUserName = noConfigString;
        private string localUserName = noConfigString;
        private string defaultTestPwd = noConfigString;
        //Form Controls
        private List<ComboBox> computerControlComboBoxList = new List<ComboBox>(5);
        private List<ListView> externalSitesControlListViewList = new List<ListView>(5);
        private List<ListView> internalSitesControlListViewList = new List<ListView>(5);
        //Lists
        private ArrayList adIpAddressList = new ArrayList();
        private ArrayList esxIpList = new ArrayList();
        private ArrayList rhServerList = new ArrayList();
        private ArrayList computerListToTestFromList = new ArrayList(new string[] { noADConfigString });
        private ArrayList externalSiteList = new ArrayList(new string[] { noConfigString });
        private ArrayList internalSiteList = new ArrayList(new string[] { noConfigString });
        private ArrayList configSettingsLineList = new ArrayList(StringResources.configSettingsLineList.Split(','));
        private ArrayList fullVMListFromESX = new ArrayList();
        private ArrayList PowerOnPrimariesDefaultList = new ArrayList(StringResources.powerOnPrimaries.Split(','));
        //Agents
        private EsxAgent esxAgent;
        private ADAgent adAgent;

        //END of VARIABLES---------------------------------------------------------------------------------------------------------------------------------//

        //MAIN BEGIN---------------------------------------------------------------------------------------------------------------------------------------//
        public MainInterface(string wordIn, ADAgent adAgentIn) {
            adAgent = adAgentIn;
            //set Window Position
            this.CenterToScreen();
            //check if test password is set, if not, get input from user
            defaultTestPwd = wordIn;
            if (defaultTestPwd.Equals("")) {
                defaultTestPwd = Interaction.InputBox("You have logged in locally. Please enter the default test password for the accounts in your domain: ", defaultPassMessageWindowTitle);
            }
            //Begin Loading GUI
            InitializeComponent();
            //load settings and clean settings file
            SupportFunctions.CleanConfig(configurationFilePath);
            CheckSettingsFiles();
            //After loading config, start agents
            ConnectESX();
            //Get List of ESX VMs from ESX
            AddVMsToFullESXList();
            //Build Menu
            BuildTreeViewMenu();
        }
        public void startProcess(string processName, string arguments, Boolean runSynchronously) {
            //processes used Notepad.exe and PowerShell.exe//
            Process process = new Process();
            ProcessStartInfo startInfoPowerShell = new ProcessStartInfo();
            startInfoPowerShell.WindowStyle = ProcessWindowStyle.Normal;
            startInfoPowerShell.FileName = processName;
            startInfoPowerShell.Arguments = (@arguments);
            process.StartInfo = startInfoPowerShell;
            process.Start();
            if (runSynchronously) { process.WaitForExit(); }
        }
        //---------------------------------Beginning of Interface Builders---------------------------------------------------------------------------------//
        #region Interface Builders
        //Main Menu----------------------------------------------->
        private void BuildTreeViewMenu() {
            //Create menu lists
            ArrayList menuTopList = new ArrayList(StringResources.topMenuList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            ArrayList menuSQTTestItems = new ArrayList(StringResources.menuSQTTestItems.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            ArrayList menuVMFunctionsItems = new ArrayList(StringResources.menuVMFunctionsItems.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            ArrayList menuConfigManagementItems = new ArrayList(StringResources.menuConfigManagementItems.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            ArrayList menuSettingsItems = new ArrayList(StringResources.menuSettingsItems.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            ArrayList listToUse = new ArrayList();
            //Create Top list
            foreach (String item in menuTopList) {
                this.MainMenuTreeView.Nodes.Add(item);
            }
            //Add list items for each list
            foreach (TreeNode node in MainMenuTreeView.Nodes) {
                if (node.Text.Equals("Tests")) {
                    listToUse = menuSQTTestItems;
                }
                if (node.Text.Equals("Virtual Machine Functions")) {
                    listToUse = menuVMFunctionsItems;
                }
                if (node.Text.Equals("Configuration Management")) {
                    listToUse = menuConfigManagementItems;
                }
                if (node.Text.Equals("Deck Settings")) {
                    listToUse = menuSettingsItems;
                }
                foreach (String item in listToUse) {
                    node.Nodes.Add(item);
                }
            }
        }
        //Sub Menu Info Pages------------------------------------->
        private void BuildWarningScreenInterface() {
            Label WarningScreenLabel = new Label();
            WarningScreenLabel.Text = "\n\n\n\nWARNING!\nTHIS HERE IS SOME GOVERNMENT PROPERTIES!";
            WarningScreenLabel.AutoSize = true;
            WarningScreenLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            formInputWindowTableLayoutPanel.Controls.Add(WarningScreenLabel, 0, 0);
            formInputWindowTableLayoutPanel.Margin = new Padding(0);
            formInputWindowTableLayoutPanel.Padding = new Padding(0);
            formInputWindowTableLayoutPanel.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
        }
        private void BuildVirtualMachineFunctionsInterface() {
            Label VirtualMachineFunctionsLabel = new Label();
            VirtualMachineFunctionsLabel.Text = "\n\n\n\nThese items are related to ESXi Management. \n\n\n\n(List and quickly describe items)";
            VirtualMachineFunctionsLabel.AutoSize = false;
            VirtualMachineFunctionsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            VirtualMachineFunctionsLabel.Height = 200;
            VirtualMachineFunctionsLabel.Width = 300;
            formInputWindowTableLayoutPanel.Controls.Add(VirtualMachineFunctionsLabel, 0, 0);
        }
        private void BuildConfigurationManagementInterface() {
            Label ConfigurationManagementLabel = new Label();
            ConfigurationManagementLabel.Text = "\n\n\n\nThese items are related to Configuration Management of the Deck. \n\n\n\n(List and quickly describe items)";
            ConfigurationManagementLabel.AutoSize = false;
            ConfigurationManagementLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            ConfigurationManagementLabel.Height = 200;
            ConfigurationManagementLabel.Width = 300;
            formInputWindowTableLayoutPanel.Controls.Add(ConfigurationManagementLabel, 0, 0);
        }
        private void BuildTestsInterface() {
            Label SQTTestsLabel = new Label();
            SQTTestsLabel.Text = "\n\n\n\nThese items are related to Automated Tests. \n\n\n\n(List and quickly describe items)";
            SQTTestsLabel.AutoSize = false;
            SQTTestsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            SQTTestsLabel.Height = 200;
            SQTTestsLabel.Width = 300;
            formInputWindowTableLayoutPanel.Controls.Add(SQTTestsLabel, 0, 0);
            formInputWindowTableLayoutPanel.Margin = new Padding(0);
            formInputWindowTableLayoutPanel.Padding = new Padding(0);
            formInputWindowTableLayoutPanel.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
        }
        private void BuildDeckSettingsInterface() {
            Label SQTDeckSettingsLabel = new Label();
            SQTDeckSettingsLabel.Text = "\n\n\n\nThese items are related to Settings that allow communication with the domain. \n\n\n\n(List and quickly describe items)";
            SQTDeckSettingsLabel.AutoSize = false;
            SQTDeckSettingsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            SQTDeckSettingsLabel.Height = 200;
            SQTDeckSettingsLabel.Width = 300;
            formInputWindowTableLayoutPanel.Controls.Add(SQTDeckSettingsLabel, 0, 0);
        }
        //Sub Menus Items-Virtual Machine Functions---------------->
        private void BuildPowerOnMachinesInterface() {
            #region Interface Control Declarations and Settings
            Label powerOnVMsLabel = new Label();
            powerOnVMsLabel.Text = "Power On Virtual Machines";
            powerOnVMsLabel.Font = new Font("Comic Sans MS", 12);
            powerOnVMsLabel.AutoSize = true;
            powerOnVMsLabel.TextAlign = ContentAlignment.TopLeft;

            Label primariesLabel = new Label();
            primariesLabel.Text = "Primary Virtual Machines";
            primariesLabel.Font = new Font("Comic Sans MS", 10);
            primariesLabel.AutoSize = true;
            primariesLabel.TextAlign = ContentAlignment.TopLeft;

            Label secondariesLabel = new Label();
            secondariesLabel.Text = "Secondary Virtual Machines";
            secondariesLabel.Font = new Font("Comic Sans MS", 10);
            secondariesLabel.AutoSize = true;
            secondariesLabel.TextAlign = ContentAlignment.TopLeft;

            Button selectAllPrimariesBtn = new Button();
            selectAllPrimariesBtn.Text = "Select All/Deselect All";
            selectAllPrimariesBtn.AutoSize = true;

            Button selectAllSecondariesBtn = new Button();
            selectAllSecondariesBtn.Text = "Select All/Deselect All";
            selectAllSecondariesBtn.AutoSize = true;

            Button powerOnMachinesBtn = new Button();
            powerOnMachinesBtn.Text = "Power On Machines";
            powerOnMachinesBtn.AutoSize = true;

            ListView powerOnPrimariesListView = new ListView();
            powerOnPrimariesListView.View = View.Details;
            powerOnPrimariesListView.CheckBoxes = true;
            powerOnPrimariesListView.Width = 175;
            powerOnPrimariesListView.Height = 350;

            powerOnPrimariesListView.Columns.Add("Primary Servers");
            powerOnPrimariesListView.Columns[0].Width = 175;

            ListView powerOnSecondariesListView = new ListView();
            powerOnSecondariesListView.View = View.Details;
            powerOnSecondariesListView.CheckBoxes = true;
            powerOnSecondariesListView.Width = 175;
            powerOnSecondariesListView.Height = 350;
            powerOnSecondariesListView.Sorting = SortOrder.Ascending;
            powerOnSecondariesListView.Columns.Add("Secondary Servers");
            powerOnSecondariesListView.Columns[0].Width = 175;

            //Add default primary servers list to list view, leave unchecked
            foreach (string primaryName in PowerOnPrimariesDefaultList) {
                powerOnPrimariesListView.Items.Add(primaryName).Checked = false;
            }
            //check VM List to see if it is empty
            if (fullVMListFromESX.Count == 0) { AddVMsToFullESXList(); }
            //Mark found Primary servers with a check and add all others to Secondary server list view
            foreach (VirtualMachine vm in fullVMListFromESX) {
                Boolean isSecondary = true;
                int counter = 0;
                foreach (string primaryName in PowerOnPrimariesDefaultList) {
                    if (vm.Name.Contains(primaryName)) {
                        isSecondary = false;
                        powerOnPrimariesListView.Items[counter].Checked = true;
                    }
                    counter++;
                }
                if (isSecondary) {
                    powerOnSecondariesListView.Items.Add(new ListViewItem(vm.Name.Split('_')[2])).Checked = true;
                }
            }
            //Remove Primary Servers that weren't found in ESX Host
            foreach (ListViewItem primaryServer in powerOnPrimariesListView.Items) {
                if (!primaryServer.Checked) { powerOnPrimariesListView.Items.Remove(primaryServer); }
            }

            TableLayoutPanel PowerOnMachinesTableLayoutPanel = new TableLayoutPanel();
            PowerOnMachinesTableLayoutPanel.Controls.Add(powerOnVMsLabel, 0, 0);
            PowerOnMachinesTableLayoutPanel.AutoSize = true;
            PowerOnMachinesTableLayoutPanel.Anchor = AnchorStyles.Left;
            PowerOnMachinesTableLayoutPanel.Anchor = AnchorStyles.Top;
            PowerOnMachinesTableLayoutPanel.Margin = new Padding(0);
            PowerOnMachinesTableLayoutPanel.Padding = new Padding(0);

            PowerOnMachinesTableLayoutPanel.Controls.Add(primariesLabel, 0, 1);
            PowerOnMachinesTableLayoutPanel.Controls.Add(secondariesLabel, 1, 1);

            PowerOnMachinesTableLayoutPanel.Controls.Add(powerOnPrimariesListView, 0, 2);
            PowerOnMachinesTableLayoutPanel.Controls.Add(powerOnSecondariesListView, 1, 2);

            formInputWindowTableLayoutPanel.Controls.Add(PowerOnMachinesTableLayoutPanel, 0, 0);
            #endregion

            #region Event Handlers
            selectAllPrimariesBtn.Click += delegate { };
            selectAllSecondariesBtn.Click += delegate { };
            powerOnMachinesBtn.Click += delegate { };
            #endregion
        }
        private void BuildPowerOffMachinesInterface() {
            Button powerOffMachinesBtn = new Button();
            powerOffMachinesBtn.Text = "Power Off Machines";
            powerOffMachinesBtn.AutoSize = true;

            TableLayoutPanel PowerOffMachinesTableLayoutPanel = new TableLayoutPanel();
            PowerOffMachinesTableLayoutPanel.Controls.Add(powerOffMachinesBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(PowerOffMachinesTableLayoutPanel, 0, 0);
        }
        private void BuildCheckVMToolsInterface() {
            Button checkVMToolsBtn = new Button();
            checkVMToolsBtn.Text = "Check VM Tools";
            checkVMToolsBtn.AutoSize = true;

            TableLayoutPanel CheckVMToolsTableLayoutPanel = new TableLayoutPanel();
            CheckVMToolsTableLayoutPanel.Controls.Add(checkVMToolsBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(CheckVMToolsTableLayoutPanel, 0, 0);
        }
        private void BuildSnapshotAllVMsInterface() {
            Button snapShotALLVMsBtn = new Button();
            snapShotALLVMsBtn.Text = "Snapshot All Vitrual Machines";
            snapShotALLVMsBtn.AutoSize = true;

            TableLayoutPanel snapShotALLVMsTableLayoutPanel = new TableLayoutPanel();
            snapShotALLVMsTableLayoutPanel.Controls.Add(snapShotALLVMsBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(snapShotALLVMsTableLayoutPanel, 0, 0);
        }
        private void BuildExportDeckInterface() {
            Button exportDeckBtn = new Button();
            exportDeckBtn.Text = "Export Deck";
            exportDeckBtn.AutoSize = true;

            TableLayoutPanel ExportDeckTableLayoutPanel = new TableLayoutPanel();
            ExportDeckTableLayoutPanel.Controls.Add(exportDeckBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(ExportDeckTableLayoutPanel, 0, 0);
        }
        //Sub Menus Items-Configuration Management----------------->
        private void BuildDomainPingTestInterface() {
            Button DomainPingTestBtn = new Button();
            DomainPingTestBtn.Text = "Start Ping Test";
            DomainPingTestBtn.AutoSize = true;

            TableLayoutPanel DomainPingTestTableLayoutPanel = new TableLayoutPanel();
            DomainPingTestTableLayoutPanel.Controls.Add(DomainPingTestBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(DomainPingTestTableLayoutPanel, 0, 0);
        }
        private void BuildCheckRBACInterface() {
            Button CheckRBACBtn = new Button();
            CheckRBACBtn.Text = "Check RBAC";
            CheckRBACBtn.AutoSize = true;

            TableLayoutPanel CheckRBACTableLayoutPanel = new TableLayoutPanel();
            CheckRBACTableLayoutPanel.Controls.Add(CheckRBACBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(CheckRBACTableLayoutPanel, 0, 0);
        }
        private void BuildCheckJavaVersioningInterface() {
            Button CheckJavaVersioningBtn = new Button();
            CheckJavaVersioningBtn.Text = "Check Java Versioning";
            CheckJavaVersioningBtn.AutoSize = true;

            TableLayoutPanel CheckJavaVersioningTableLayoutPanel = new TableLayoutPanel();
            CheckJavaVersioningTableLayoutPanel.Controls.Add(CheckJavaVersioningBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(CheckJavaVersioningTableLayoutPanel, 0, 0);
        }
        private void BuildCheckAllServicesInterface() {
            Button checkAllServicesBtn = new Button();
            checkAllServicesBtn.Text = "Check All Services";
            checkAllServicesBtn.AutoSize = true;

            TableLayoutPanel CheckAllServicesTableLayoutPanel = new TableLayoutPanel();
            CheckAllServicesTableLayoutPanel.Controls.Add(checkAllServicesBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(CheckAllServicesTableLayoutPanel, 0, 0);
        }
        private void BuildGetRegistryDataInterface() {
            Button getRegistryDataBtn = new Button();
            getRegistryDataBtn.Text = "Get Registry Data";
            getRegistryDataBtn.AutoSize = true;

            TableLayoutPanel GetRegistryDataTableLayoutPanel = new TableLayoutPanel();
            GetRegistryDataTableLayoutPanel.Controls.Add(getRegistryDataBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(GetRegistryDataTableLayoutPanel, 0, 0);
        }
        //Sub Menus Items-SQT TESTS------------------------------->
        private void BuildInternalWebsiteTestsInterface() {
            Button InternalWebsiteTestsBtn = new Button();
            InternalWebsiteTestsBtn.Text = "Start Internal Website Tests";
            InternalWebsiteTestsBtn.AutoSize = true;

            TableLayoutPanel InternalWebsiteTestsTableLayoutPanel = new TableLayoutPanel();
            InternalWebsiteTestsTableLayoutPanel.Controls.Add(InternalWebsiteTestsBtn, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(InternalWebsiteTestsTableLayoutPanel, 0, 0);
        }
        private void BuildExternalSiteTestInterface() {
            int i;
            for (i = 0; i < externalSiteTestLines; i++) {
                BuildSingleExternalSiteTestLine(i, computerListToTestFromList, externalSiteList);
            }

            //create External test controls

            //Interface Control builders------------------------------------------------------------------------------------>>>
            #region Interface Control Declarationsand Settings
            Button AddTestLineBtn = new Button();
            AddTestLineBtn.Text = "Add New Line";
            AddTestLineBtn.AutoSize = true;

            Button RemoveTestLineBtn = new Button();
            RemoveTestLineBtn.Text = "Remove Line";
            RemoveTestLineBtn.AutoSize = true;

            Button RunTestsBtn = new Button();
            RunTestsBtn.Text = "Run Tests";
            RunTestsBtn.AutoSize = true;

            TableLayoutPanel AddRemoveTestLinesTableLayoutPanel = new TableLayoutPanel();
            AddRemoveTestLinesTableLayoutPanel.Controls.Add(AddTestLineBtn, 0, 0);
            AddRemoveTestLinesTableLayoutPanel.Controls.Add(RemoveTestLineBtn, 1, 0);
            AddRemoveTestLinesTableLayoutPanel.Controls.Add(RunTestsBtn, 2, 0);
            AddRemoveTestLinesTableLayoutPanel.Width = 600;
            AddRemoveTestLinesTableLayoutPanel.Height = 30;

            Label BottomDivider = new Label();
            BottomDivider.Text = ("End of TESTS------------------------------------------------------------------------------------------------------------------------------------>>");
            BottomDivider.Width = 600;
            BottomDivider.Height = 15;


            //Add all Controls for External Website Test

            formInputWindowTableLayoutPanel.Controls.Add(BottomDivider, 0, (i + 1));
            formInputWindowTableLayoutPanel.Controls.Add(AddRemoveTestLinesTableLayoutPanel, 0, (i + 2));
            #endregion

            //Event handlers------------------------------------------------------------------------------------>>>
            #region Event handlers
            AddTestLineBtn.Click += delegate {
                if (externalSiteTestLines < 5) {
                    externalSiteTestLines++;
                    formInputWindowTableLayoutPanel.Controls.Clear();
                    BuildExternalSiteTestInterface();
                } else {
                    MessageBox.Show(new Form { TopMost = true },"You cannot run than five tests!", actionCanceledMessageWindowTitle);
                }
            };

            RemoveTestLineBtn.Click += delegate {
                if (externalSiteTestLines > 1) {
                    externalSiteTestLines--;
                    externalSitesControlListViewList.RemoveAt(externalSitesControlListViewList.Count - 1);
                    formInputWindowTableLayoutPanel.Controls.Clear();
                    BuildExternalSiteTestInterface();
                } else {
                    MessageBox.Show(new Form { TopMost = true },"You cannot run less than one test!", actionCanceledMessageWindowTitle);
                }
            };

            RunTestsBtn.Click += delegate {
                int testCounter = 1;
                List<string> checkedSites = new List<string>();
                StringBuilder sb = new StringBuilder();
                //Create/clear Log File

                File.Create(externalSiteTestLogFilePath).Close();
                File.WriteAllText(externalSiteTestLogFilePath, "");

                foreach (ListView lv in externalSitesControlListViewList) {
                    ComboBox cb = computerControlComboBoxList[testCounter - 1];
                    if (cb.Text.Equals("Choose Workstation To Test From")) {
                        MessageBox.Show(new Form { TopMost = true },"Error! You must choose a computer to test from for test: " + testCounter, externalSiteTestManagerMessageWindowTitle);
                        return;
                    }
                    if (lv.CheckedItems.Count == 0) {
                        MessageBox.Show(new Form { TopMost = true },"Error! You must choose at least one site to connect to for test: " + testCounter, externalSiteTestManagerMessageWindowTitle);
                        return;
                    }
                    sb.Clear();
                    foreach (ListViewItem lvi in lv.Items) {
                        if (lvi.Checked == true) {
                            Console.WriteLine("Running Test: " + testCounter + " From Computer: " + cb.Text + " Connecting To: " + lvi.Text);
                            sb.Append("\"" + lvi.Text + "\",");
                        }
                    }
                    sb.Remove(sb.Length - 1, 1);
                    Console.WriteLine(sb);
                    testCounter++;
                    if (cb.Text.Equals("All Worstations")) {
                        cb.Text = "KMS";
                    }
                    try {
                        //Start PowerShell Script for external testing
                        startProcess("powershell.exe", (".\\SQTExternalSiteTest.ps1 -TestFromComputer " + cb.Text + " -SiteToConnectTo " + sb), true);
                    } catch (Exception e) {
                        MessageBox.Show(new Form { TopMost = true },"There was an error running the PowerShell script: " + e.Message, externalSiteTestManagerMessageWindowTitle);
                    }
                }
                startProcess("notepad.exe", externalSiteTestLogFilePath, true);
            };
            #endregion
        }
        private void BuildSingleExternalSiteTestLine(int lineCounter, ArrayList ComputerToTestFromList, ArrayList ExternalSiteList) {

            TableLayoutPanel SingleTestLinePanel = new TableLayoutPanel();
            SingleTestLinePanel.Width = 600;
            SingleTestLinePanel.Height = 218;
            SingleTestLinePanel.VerticalScroll.Enabled = true;

            Label TopDivider = new Label();
            TopDivider.Text = ("Start of TEST NO.{" + (lineCounter + 1) + "}-------------------------------------------------------------------------------------------------------------->>");
            TopDivider.Width = 600;
            TopDivider.Height = 15;

            Label ComputerToTestFromLabel = new Label();
            ComputerToTestFromLabel.Text = "Computer/Worstation to Connect From:";
            ComputerToTestFromLabel.Width = 600;
            ComputerToTestFromLabel.Height = 15;

            Label SitesToTestLabel = new Label();
            SitesToTestLabel.Text = "Sites To Connect To:";
            SitesToTestLabel.Width = 600;
            SitesToTestLabel.Height = 15;

            Button SelectAllBtn = new Button();
            SelectAllBtn.Text = "Select All";

            Button DeselectAllBtn = new Button();
            DeselectAllBtn.Text = "Deselect All";

            TableLayoutPanel SelectButtonsTableLayoutPanel = new TableLayoutPanel();
            SelectButtonsTableLayoutPanel.Controls.Add(SelectAllBtn, 0, 0);
            SelectButtonsTableLayoutPanel.Controls.Add(DeselectAllBtn, 1, 0);
            SelectButtonsTableLayoutPanel.Width = 600;
            SelectButtonsTableLayoutPanel.Height = 30;

            ComboBox ComputerToTestFromComboBox;
            try {
                ComputerToTestFromComboBox = computerControlComboBoxList[lineCounter];
            } catch (Exception e) {
                //MessageBox.Show(new Form { TopMost = true },"Error:" + e.Message); 
                ComputerToTestFromComboBox = new ComboBox();
                foreach (String s in ComputerToTestFromList) {
                    ComputerToTestFromComboBox.Items.Add(s);
                }
                ComputerToTestFromComboBox.Width = 240;
                ComputerToTestFromComboBox.SelectedIndex = 0;
                ComputerToTestFromComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                computerControlComboBoxList.Add(ComputerToTestFromComboBox);
            }

            ListView SitesToTestListView;
            try {
                SitesToTestListView = externalSitesControlListViewList[lineCounter];
            } catch (Exception e) {
                //MessageBox.Show(new Form { TopMost = true },"Error:" + e.Message); 
                SitesToTestListView = new ListView();
                SitesToTestListView.CheckBoxes = true;
                foreach (String s in ExternalSiteList) {
                    SitesToTestListView.Items.Add(s);
                }
                SitesToTestListView.View = View.List;
                SitesToTestListView.Width = 240;
                SitesToTestListView.Height = 90;
                externalSitesControlListViewList.Add(SitesToTestListView);
            }

            SingleTestLinePanel.Controls.Add(TopDivider, 0, 0);
            SingleTestLinePanel.Controls.Add(ComputerToTestFromLabel, 0, 1);
            SingleTestLinePanel.Controls.Add(ComputerToTestFromComboBox, 0, 2);
            SingleTestLinePanel.Controls.Add(SitesToTestLabel, 0, 3);
            SingleTestLinePanel.Controls.Add(SelectButtonsTableLayoutPanel, 0, 4);
            SingleTestLinePanel.Controls.Add(SitesToTestListView, 0, 5);
            formInputWindowTableLayoutPanel.Controls.Add(SingleTestLinePanel, 0, lineCounter);

            SelectAllBtn.Click += delegate {
                foreach (ListViewItem item in SitesToTestListView.Items) {
                    item.Checked = true;
                }
            };
            DeselectAllBtn.Click += delegate {
                foreach (ListViewItem item in SitesToTestListView.Items) {
                    item.Checked = false;
                }
            };
        }
        //Sub Menus Items-SQT Deck Settings------------------------->
        private ArrayList BuildServerSettingsInterface() {
            //Interface Control builders------------------------------------------------------------------------------------>>>
            #region Interface Control Declarationsand Settings
            Label ESXIPSettingsTitleLabel = new Label();
            ESXIPSettingsTitleLabel.Text = "ESX Host Settings";
            ESXIPSettingsTitleLabel.Font = new Font("Comic Sans MS", 12);
            ESXIPSettingsTitleLabel.AutoSize = true;
            ESXIPSettingsTitleLabel.TextAlign = ContentAlignment.TopLeft;

            Label ESXIPListLabel = new Label();
            ESXIPListLabel.Text = "ESX Host IP Address List:";
            ESXIPListLabel.AutoSize = true;
            ESXIPListLabel.TextAlign = ContentAlignment.TopLeft;

            TextBox ESXIPListTextBox = new TextBox();
            StringBuilder esxIPListSB = new StringBuilder();
            foreach (string ip in esxIpList) {
                if (!ip.Equals("")) {
                    esxIPListSB.Append(ip + ";");
                };
            }
            ESXIPListTextBox.Text = esxIPListSB.ToString();
            ESXIPListTextBox.AutoSize = false;
            ESXIPListTextBox.Multiline = true;
            ESXIPListTextBox.WordWrap = true;
            ESXIPListTextBox.AcceptsReturn = true;
            ESXIPListTextBox.ScrollBars = ScrollBars.Vertical;
            ESXIPListTextBox.Width = 300;
            ESXIPListTextBox.Height = 75;
            ESXIPListTextBox.Anchor = AnchorStyles.Left;
            ESXIPListTextBox.TextAlign = HorizontalAlignment.Left;

            Label ESXIPListInstructionsLabel = new Label();
            ESXIPListInstructionsLabel.Text = "*Enter Ip Adresses with a ';' as a delimiter";
            ESXIPListInstructionsLabel.AutoSize = true;
            ESXIPListInstructionsLabel.TextAlign = ContentAlignment.TopLeft;

            Button saveESXIPSettingsBtn = new Button();
            saveESXIPSettingsBtn.Text = "Save ESX IP Settings";
            saveESXIPSettingsBtn.AutoSize = true;
            saveESXIPSettingsBtn.Anchor = (AnchorStyles.Left | AnchorStyles.Top);

            Label rhServerListLabel = new Label();
            rhServerListLabel.Text = "RedHat Server Address List:";
            rhServerListLabel.AutoSize = true;
            rhServerListLabel.TextAlign = ContentAlignment.TopLeft;

            TextBox rhServerListTextBox = new TextBox();
            StringBuilder redHatListSB = new StringBuilder();
            foreach (string ip in rhServerList) {
                if (!ip.Equals("")) {
                    redHatListSB.Append(ip + ";");
                };
            }
            rhServerListTextBox.Text = redHatListSB.ToString();
            rhServerListTextBox.AutoSize = false;
            rhServerListTextBox.Multiline = true;
            rhServerListTextBox.WordWrap = true;
            rhServerListTextBox.AcceptsReturn = true;
            rhServerListTextBox.ScrollBars = ScrollBars.Vertical;
            rhServerListTextBox.Width = 300;
            rhServerListTextBox.Height = 75;
            rhServerListTextBox.Anchor = AnchorStyles.Left;
            rhServerListTextBox.TextAlign = HorizontalAlignment.Left;

            Label rhServerListInstructionsLabel = new Label();
            rhServerListInstructionsLabel.Text = "*Enter Ip Adresses with a ';' as a delimiter";
            rhServerListInstructionsLabel.AutoSize = true;
            rhServerListInstructionsLabel.TextAlign = ContentAlignment.TopLeft;

            Button saveRHServerSettingsBtn = new Button();
            saveRHServerSettingsBtn.Text = "Save RedHat Server Settings";
            saveRHServerSettingsBtn.AutoSize = true;
            saveRHServerSettingsBtn.Anchor = (AnchorStyles.Left | AnchorStyles.Top);

            int counter = 0;

            formInputWindowTableLayoutPanel.Controls.Add(ESXIPSettingsTitleLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(ESXIPListLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(ESXIPListTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(ESXIPListInstructionsLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(saveESXIPSettingsBtn, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(rhServerListLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(rhServerListTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(rhServerListInstructionsLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(saveRHServerSettingsBtn, 0, counter++);

            #endregion
            //EVENT HANDLERS------------------------------------------------------------------------------------>>>
            #region Event Handlers
            saveESXIPSettingsBtn.Click += delegate {
                DialogResult userResponse = MessageBox.Show(new Form { TopMost = true },"I will now save your ESX Host IP addresses!\nWhen I save this list, any invalid Ips will be removed. Default values remain until overwritten with at least one valid address.", ipAddressValidatorMessageWindowTitle);
                //variables
                String uncheckedIpString = ESXIPListTextBox.Text;
                ArrayList uncheckedIPList = new ArrayList();
                ArrayList checkedIPList = new ArrayList();
                uncheckedIPList.AddRange(uncheckedIpString.Split(';'));
                //Create a list of valid ips
                foreach (string ip in uncheckedIPList) {
                    Console.WriteLine(ip);
                    if (SQTSupportFunctions.SupportFunctions.ValidateIp(ip)) {
                        checkedIPList.Add(ip);
                    } else {
                        continue;
                    }
                }
                //check list length for no valid entries
                if (checkedIPList.Count > 0) {
                    //Check for save file
                    if (!File.Exists(configurationFilePath)) {
                        File.Create(configurationFilePath).Close();
                    }
                    //Read file and get specific line to replace
                    StreamReader configFileReader = new StreamReader(configurationFilePath);
                    string lineToReplace = "";
                    StringBuilder newConfigLineSB = new StringBuilder();
                    configurationFilePath = Path.GetFullPath(configurationFilePath);
                    Console.WriteLine("\nConfig File Location: " + configurationFilePath);
                    string line;

                    while ((line = configFileReader.ReadLine()) != null) {
                        if (line.StartsWith("[ESXIP]")) {
                            Console.WriteLine("Getting info... " + line);
                            lineToReplace = line;
                        }
                    }
                    configFileReader.Close();
                    //Build new line for config file string
                    newConfigLineSB.Append("[ESXIP]");
                    foreach (string ip in checkedIPList) {
                        newConfigLineSB.Append(ip + ";");
                    };
                    String configFileText = File.ReadAllText(configurationFilePath);
                    if (!lineToReplace.Equals("")) {
                        //Replace old config line and set text file contents  
                        configFileText = configFileText.Replace(lineToReplace, newConfigLineSB.ToString());
                    } else {
                        StringBuilder addLineSB = new StringBuilder(File.ReadAllText(configurationFilePath));
                        addLineSB.Append(newConfigLineSB.ToString());
                        configFileText = addLineSB.ToString();
                    }
                    try {
                        File.WriteAllText(configurationFilePath, configFileText);
                    } catch (Exception e) {
                        MessageBox.Show(new Form { TopMost = true },"There was an error trying to write to your config file!\nPlease check the file isn't being used by another program and retry save action.\n" + e.Message, ipAddressValidatorMessageWindowTitle);
                    }
                } else {
                    MessageBox.Show(new Form { TopMost = true },"None of the addresses you tried to save were valid! Try again!", ipAddressValidatorMessageWindowTitle);
                }
                //Reload New Config Values                   
                CheckSettingsFiles();
                StringBuilder reloadedIpList = new StringBuilder();
                foreach (string ip in esxIpList) {
                    if (!ip.Equals("")) {
                        reloadedIpList.Append(ip + ";");
                    };
                }
                string newESXIpList = reloadedIpList.ToString();
                ESXIPListTextBox.Text = newESXIpList;
            };
            saveRHServerSettingsBtn.Click += delegate {
                DialogResult userResponse = MessageBox.Show(new Form { TopMost = true },"I will now save your RedHat Server IP Addresses!\nWhen I save this list, any invalid Ips will be removed. Default values remain until overwritten with at least one valid address.", ipAddressValidatorMessageWindowTitle);
                //variables
                String uncheckedIpString = rhServerListTextBox.Text;
                ArrayList uncheckedIPList = new ArrayList();
                ArrayList checkedIPList = new ArrayList();
                uncheckedIPList.AddRange(uncheckedIpString.Split(';'));
                //Create a list of valid ips
                foreach (string ip in uncheckedIPList) {
                    Console.WriteLine(ip);
                    if (SQTSupportFunctions.SupportFunctions.ValidateIp(ip)) {
                        checkedIPList.Add(ip);
                    } else {
                        continue;
                    }
                }
                //check list length for no valid entries
                if (checkedIPList.Count > 0) {
                    //Check for save file
                    if (!File.Exists(configurationFilePath)) {
                        File.Create(configurationFilePath).Close();
                    }
                    //Read file and get specific line to replace
                    StreamReader configFileReader = new StreamReader(configurationFilePath);
                    string lineToReplace = "";
                    StringBuilder newConfigLineSB = new StringBuilder();
                    configurationFilePath = Path.GetFullPath(configurationFilePath);
                    Console.WriteLine("\nConfig File Location: " + configurationFilePath);
                    string line;

                    while ((line = configFileReader.ReadLine()) != null) {
                        if (line.StartsWith("[REDHAT]")) {
                            Console.WriteLine("Getting info... " + line);
                            lineToReplace = line;
                        }
                    }
                    configFileReader.Close();
                    //Build new line for config file string
                    newConfigLineSB.Append("[REDHAT]");
                    foreach (string ip in checkedIPList) {
                        newConfigLineSB.Append(ip + ";");
                    };
                    String configFileText = File.ReadAllText(configurationFilePath);
                    if (!lineToReplace.Equals("")) {
                        //Replace old config line and set text file contents  
                        configFileText = configFileText.Replace(lineToReplace, newConfigLineSB.ToString());
                    } else {
                        StringBuilder addLineSB = new StringBuilder(File.ReadAllText(configurationFilePath));
                        addLineSB.Append(newConfigLineSB.ToString());
                        configFileText = addLineSB.ToString();
                    }
                    try {
                        File.WriteAllText(configurationFilePath, configFileText);
                    } catch (Exception e) {
                        MessageBox.Show(new Form { TopMost = true },"There was an error trying to write to your config file!\nPlease check the file isn't being used by another program and retry save action.\n" + e.Message, ipAddressValidatorMessageWindowTitle);
                    }
                } else {
                    MessageBox.Show(new Form { TopMost = true },"None of the addresses you tried to save were valid! Try again!", ipAddressValidatorMessageWindowTitle);
                }
                //Reload New Config Values                   
                CheckSettingsFiles();
                StringBuilder reloadedRHServerList = new StringBuilder();
                foreach (string ip in rhServerList) {
                    if (!ip.Equals("")) {
                        reloadedRHServerList.Append(ip + ";");
                    };
                }
                string newRHServerList = reloadedRHServerList.ToString();
                rhServerListTextBox.Text = newRHServerList;
            };
            #endregion
            //put buttons in a list to be passed out for validation automation
            ArrayList validationButtons = new ArrayList();
            validationButtons.Add(saveESXIPSettingsBtn);
            validationButtons.Add(saveRHServerSettingsBtn);

            return validationButtons;
        }
        private Button BuildShipAndUserSettingsInterface() {
            //Interface Control builders------------------------------------------------------------------------------------>>>
            #region Interface Control Declarations and Settings
            Hashtable allTextBoxHT = new Hashtable();
            Hashtable windowsUserTextBoxHT = new Hashtable();
            Hashtable LinuxUserTextBoxHT = new Hashtable();

            Label shipSettingsTitleLabel = new Label();
            shipSettingsTitleLabel.Text = "Ship Settings";
            shipSettingsTitleLabel.Font = new Font("Comic Sans MS", 12);
            shipSettingsTitleLabel.AutoSize = true;
            shipSettingsTitleLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Label shipHullNumberLabel = new Label();
            shipHullNumberLabel.Text = "Ship Hull Number(ex:ncc1031):";
            shipHullNumberLabel.AutoSize = true;
            shipHullNumberLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox shipHullNumberTextBox = new TextBox();
            shipHullNumberTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(hullNumber);
            shipHullNumberTextBox.AutoSize = false;
            shipHullNumberTextBox.Width = 300;
            shipHullNumberTextBox.Anchor = AnchorStyles.Left;
            shipHullNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("shipHullNumberTextBox", shipHullNumberTextBox);

            Label shipADIpAddressLabel = new Label();
            shipADIpAddressLabel.Text = "Active Directory IP Addresses:";
            shipADIpAddressLabel.AutoSize = true;
            shipADIpAddressLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox shipADIpAddressTextBox = new TextBox();
            StringBuilder sb = new StringBuilder();
            foreach (string ip in adIpAddressList) {
                if (!ip.Equals("")) {
                    sb.Append(ip + ";");
                };
            }
            shipADIpAddressTextBox.Text = sb.ToString();
            shipADIpAddressTextBox.AutoSize = false;
            shipADIpAddressTextBox.Width = 300;
            shipADIpAddressTextBox.Anchor = AnchorStyles.Left;
            shipADIpAddressTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("shipADIpAddressTextBox", shipADIpAddressTextBox);

            Label userSettingsTitleLabel = new Label();
            userSettingsTitleLabel.Text = "User Settings";
            userSettingsTitleLabel.Font = new Font("Comic Sans MS", 12);
            userSettingsTitleLabel.AutoSize = true;
            userSettingsTitleLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Label regularUserNameLabel = new Label();
            regularUserNameLabel.Text = "Regular User Name(ex:<hull#>\\<username>):";
            regularUserNameLabel.AutoSize = true;
            regularUserNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox regularUserNameTextBox = new TextBox();
            regularUserNameTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(regUserName);
            regularUserNameTextBox.Text = (hullNumber + "\\" + regularUserNameTextBox.Text);
            regularUserNameTextBox.AutoSize = false;
            regularUserNameTextBox.Width = 300;
            regularUserNameTextBox.Anchor = AnchorStyles.Left;
            regularUserNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("regularUserNameTextBox", regularUserNameTextBox);
            windowsUserTextBoxHT.Add("regularUserNameTextBox", regularUserNameTextBox);

            Label domainAdminNameLabel = new Label();
            domainAdminNameLabel.Text = "Domain Admin Name(ex:<hull#>\\<username>):";
            domainAdminNameLabel.AutoSize = true;
            domainAdminNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox domainAdminNameTextBox = new TextBox();
            domainAdminNameTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(domainAdminName);
            domainAdminNameTextBox.Text = (hullNumber + "\\" + domainAdminNameTextBox.Text);
            domainAdminNameTextBox.AutoSize = false;
            domainAdminNameTextBox.Width = 300;
            domainAdminNameTextBox.Anchor = AnchorStyles.Left;
            domainAdminNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("domainAdminNameTextBox", domainAdminNameTextBox);
            windowsUserTextBoxHT.Add("domainAdminNameTextBox", domainAdminNameTextBox);

            Label serverAdminNameLabel = new Label();
            serverAdminNameLabel.Text = "Server Admin Name(ex:<hull#>\\<username>):";
            serverAdminNameLabel.AutoSize = true;
            serverAdminNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox serverAdminNameTextBox = new TextBox();
            serverAdminNameTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(serverAdminName);
            serverAdminNameTextBox.Text = (hullNumber + "\\" + serverAdminNameTextBox.Text);
            serverAdminNameTextBox.AutoSize = false;
            serverAdminNameTextBox.Width = 300;
            serverAdminNameTextBox.Anchor = AnchorStyles.Left;
            serverAdminNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("serverAdminNameTextBox", serverAdminNameTextBox);
            windowsUserTextBoxHT.Add("serverAdminNameTextBox", serverAdminNameTextBox);

            Label workstationAdminNameLabel = new Label();
            workstationAdminNameLabel.Text = "Workstation Admin Name(ex:<hull#>\\<username>):";
            workstationAdminNameLabel.AutoSize = true;
            workstationAdminNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox workstationAdminNameTextBox = new TextBox();
            workstationAdminNameTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(workstationAdminName);
            workstationAdminNameTextBox.Text = (hullNumber + "\\" + workstationAdminNameTextBox.Text);
            workstationAdminNameTextBox.AutoSize = false;
            workstationAdminNameTextBox.Width = 300;
            workstationAdminNameTextBox.Anchor = AnchorStyles.Left;
            workstationAdminNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("workstationAdminNameTextBox", workstationAdminNameTextBox);
            windowsUserTextBoxHT.Add("workstationAdminNameTextBox", workstationAdminNameTextBox);

            Label linuxSSHuserNameLabel = new Label();
            linuxSSHuserNameLabel.Text = "Linux SSH User Name:";
            linuxSSHuserNameLabel.AutoSize = true;
            linuxSSHuserNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox linuxSSHuserNameTextBox = new TextBox();
            linuxSSHuserNameTextBox.Text = sshUserName;
            linuxSSHuserNameTextBox.AutoSize = false;
            linuxSSHuserNameTextBox.Width = 300;
            linuxSSHuserNameTextBox.Anchor = AnchorStyles.Left;
            linuxSSHuserNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("linuxSSHuserNameTextBox", linuxSSHuserNameTextBox);
            LinuxUserTextBoxHT.Add("linuxSSHuserNameTextBox", linuxSSHuserNameTextBox);

            Label linuxSMUserNameLabel = new Label();
            linuxSMUserNameLabel.Text = "Linux SM User Name:";
            linuxSMUserNameLabel.AutoSize = true;
            linuxSMUserNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox linuxSMUserNameTextBox = new TextBox();
            linuxSMUserNameTextBox.Text = smUserName;
            linuxSMUserNameTextBox.AutoSize = false;
            linuxSMUserNameTextBox.Width = 300;
            linuxSMUserNameTextBox.Anchor = AnchorStyles.Left;
            linuxSMUserNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("linuxSMUserNameTextBox", linuxSMUserNameTextBox);
            LinuxUserTextBoxHT.Add("linuxSMUserNameTextBox", linuxSMUserNameTextBox);

            Label esxUserNameLabel = new Label();
            esxUserNameLabel.Text = "ESX User Name:";
            esxUserNameLabel.AutoSize = true;
            esxUserNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox esxUserNameTextBox = new TextBox();
            esxUserNameTextBox.Text = esxUserName;
            esxUserNameTextBox.AutoSize = false;
            esxUserNameTextBox.Width = 300;
            esxUserNameTextBox.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            esxUserNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("esxUserNameTextBox", esxUserNameTextBox);
            LinuxUserTextBoxHT.Add("esxUserNameTextBox", esxUserNameTextBox);

            TableLayoutPanel localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel = new TableLayoutPanel();
            localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel.AutoSize = true;

            Label localUserNameLabel = new Label();
            localUserNameLabel.Text = "Local User Name:";
            localUserNameLabel.AutoSize = true;
            localUserNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Button changeLocalPassBtn = new Button();
            changeLocalPassBtn.Text = "Change Local Password";
            changeLocalPassBtn.AutoSize = true;
            changeLocalPassBtn.Font = new Font(changeLocalPassBtn.Font.FontFamily, 7);
            changeLocalPassBtn.Margin = new Padding(0, 0, 0, 0);

            TextBox locUserNameTextBox = new TextBox();
            locUserNameTextBox.Text = localUserName;
            locUserNameTextBox.AutoSize = false;
            locUserNameTextBox.Width = 300;
            locUserNameTextBox.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            locUserNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            locUserNameTextBox.Margin = new Padding(0, 2, 0, 0);
            allTextBoxHT.Add("locUserNameTextBox", locUserNameTextBox);

            TableLayoutPanel userSettingsButtonTableLayoutPanel = new TableLayoutPanel();
            userSettingsButtonTableLayoutPanel.AutoSize = true;

            Button saveUserSettingsBtn = new Button();
            saveUserSettingsBtn.Text = "Save User Settings";
            saveUserSettingsBtn.AutoSize = true;

            Button validateUserSettingsBtn = new Button();
            validateUserSettingsBtn.Text = "Validate and Save User Settings";
            validateUserSettingsBtn.AutoSize = true;

            int counter = 0;
            localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel.Controls.Add(locUserNameTextBox, 0, 0);
            localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel.Controls.Add(changeLocalPassBtn, 1, 0);
            userSettingsButtonTableLayoutPanel.Controls.Add(saveUserSettingsBtn, 0, 0);
            userSettingsButtonTableLayoutPanel.Controls.Add(validateUserSettingsBtn, 1, 0);
            formInputWindowTableLayoutPanel.Controls.Add(shipSettingsTitleLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(shipHullNumberLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(shipHullNumberTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(shipADIpAddressLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(shipADIpAddressTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(userSettingsTitleLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(regularUserNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(regularUserNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(domainAdminNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(domainAdminNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(serverAdminNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(serverAdminNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(workstationAdminNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(workstationAdminNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(linuxSSHuserNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(linuxSSHuserNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(linuxSMUserNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(linuxSMUserNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(esxUserNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(esxUserNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(localUserNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(userSettingsButtonTableLayoutPanel, 0, counter++);
            #endregion

            //EVENT HANDLERS------------------------------------------------------------------------------------>>>
            #region Event Handlers
            //checks user input for mistakes then save to configuration
            saveUserSettingsBtn.Click += delegate {

                RefreshUserListHulls(windowsUserTextBoxHT, shipHullNumberTextBox);
                CheckSettingsFiles();

                ArrayList newValuesToSave = new ArrayList();
                ArrayList errorList = new ArrayList();

                foreach (DictionaryEntry pair in allTextBoxHT) {
                    TextBox currentTB = (TextBox)allTextBoxHT[pair.Key];
                    switch (pair.Key) {
                        case "shipHullNumberTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestHullRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Ship Hull: You can only use numbers and letters for the hull!");
                            } else {
                                newValuesToSave.Add("[HULL]" + currentTB.Text.Trim());
                            }
                            break;
                        case "shipADIpAddressTextBox":
                            string line = currentTB.Text.Replace("[ADIP]", "");
                            ArrayList adIpAddressListToCheck = new ArrayList(line.Split(';'));
                            StringBuilder adIpAddressListChecked = new StringBuilder();
                            adIpAddressListChecked.Append("[ADIP]");
                            foreach (string ip in adIpAddressListToCheck) {
                                if (!ip.Equals("")) {
                                    if (!(SQTSupportFunctions.SupportFunctions.ValidateIp(ip))) {
                                        errorList.Add("Ship AD: Value entered (" + ip + ") is not an IP!");
                                    } else {
                                        adIpAddressListChecked.Append(ip + ";");
                                    }
                                }
                            }
                            newValuesToSave.Add(adIpAddressListChecked.ToString());
                            break;
                        case "regularUserNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Regula rUser Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            } else {
                                newValuesToSave.Add("[USER][REG]" + currentTB.Text.Trim());
                            }
                            break;
                        case "domainAdminNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Domain Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            } else {
                                newValuesToSave.Add("[USER][DA]" + currentTB.Text.Trim());
                            }
                            break;
                        case "serverAdminNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Server Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            } else {
                                newValuesToSave.Add("[USER][SA]" + currentTB.Text.Trim());
                            }
                            break;
                        case "workstationAdminNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Workstation Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            } else {
                                newValuesToSave.Add("[USER][WA]" + currentTB.Text.Trim());
                            }
                            break;
                        case "linuxSSHuserNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Linux SSH User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods");
                            } else {
                                newValuesToSave.Add("[USER][SSH]" + currentTB.Text.Trim());
                            }
                            break;
                        case "linuxSMUserNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text))) {
                                errorList.Add("Linux SM User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods");
                            } else {
                                newValuesToSave.Add("[USER][SM]" + currentTB.Text);
                            }
                            break;
                        case "esxUserNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("ESX User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods");
                            } else {
                                newValuesToSave.Add("[USER][ESX]" + currentTB.Text.Trim());
                            }
                            break;
                        case "locUserNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Local User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods");
                            } else {
                                newValuesToSave.Add("[USER][LOC]" + currentTB.Text.Trim());
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
                    MessageBox.Show(new Form { TopMost = true },"I need you to fix these issues:\n" + errorListSB.ToString(), hullandUserValidatorMessageWindowTitle);
                    return;
                } else {
                    //Check for save file
                    if (!File.Exists(configurationFilePath)) {
                        File.Create(configurationFilePath).Close();
                    }
                    //Read file and get specific line to replace
                    StreamReader configFileReader = new StreamReader(configurationFilePath);
                    configurationFilePath = Path.GetFullPath(configurationFilePath);
                    Console.WriteLine("\nConfig File Location: " + configurationFilePath);

                    string line;//holds read line to be read, identifies line by [xxx] value, then passes to appropriate variable for use later
                    StringBuilder newConfigLineSB = new StringBuilder();//builds new line to save

                    //Read file to get values values that aren't changed on this screen from config file
                    while ((line = configFileReader.ReadLine()) != null) {
                        //Save values that can't be modified here
                        if (line.StartsWith("[ESXIP]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                        if (line.StartsWith("[WEBTEST][EXTERNAL]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                        if (line.StartsWith("[WEBTEST][INTERNAL]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                        if (line.StartsWith("[LOCPH]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                        if (line.StartsWith("[REDHAT]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                    }
                    configFileReader.Close();

                    //Replace old config line and set text file contents
                    newValuesToSave.Sort();
                    StringBuilder configFileTextBuilder = new StringBuilder();
                    for (int i = 0; i < newValuesToSave.Count; i++) {
                        configFileTextBuilder.Append(newValuesToSave[i].ToString() + "\n");
                        Console.WriteLine("Value To Saved: " + newValuesToSave[i].ToString());
                    }
                    try {
                        File.WriteAllText(configurationFilePath, configFileTextBuilder.ToString().Trim());
                    } catch (Exception e) {
                        MessageBox.Show(new Form { TopMost = true },"There was an error trying to write to your config file!\nPlease check the file isn't being used by another program and retry save action.\n" + e.Message, ipAddressValidatorMessageWindowTitle);
                    }
                }
                formInputWindowTableLayoutPanel.Controls.Clear();
                CheckSettingsFiles();
                BuildShipAndUserSettingsInterface();
            };
            //validates Windows Users
            validateUserSettingsBtn.Click += delegate {
                MessageBox.Show(new Form { TopMost = true },"I will now save settings, remove invalid AD Addresses, and begin checking your accounts.", accountValidatorMessageWindowTitle);
                Console.WriteLine("I will now save settings, remove invalid AD Addresses, and begin check Windows Machines");
                ArrayList goodADIps = new ArrayList();
                ArrayList adIpAddressListToCheck = new ArrayList();
                ArrayList failedLoginList = new ArrayList();
                string message = "DEFAULT ERROR";
                string adUserToTestString = "TEXTBOXNULL";
                string domainAdminUser = domainAdminNameTextBox.Text.Remove(0, domainAdminNameTextBox.Text.IndexOf("\\") + 1);
                bool atLeastOneLoginSuccess = false;
                //capture and check current input box values, abort operation if one is wrong
                if (!ValidateUserAccountInput(allTextBoxHT)) {
                    return;
                };
                //WINDOWS USER Verification--------------------------------------------------------->>>
                #region Windows Verification
                Console.WriteLine("VERIFYING WINDOWS ACCOUNTS--------------------------------->>>>>>>>>>");
                //get new IP list to check
                adIpAddressListToCheck = new ArrayList(shipADIpAddressTextBox.Text.Trim().Split(';'));
                StringBuilder newGoodIpString = new StringBuilder();
                //reload agent
                SupportFunctions.ShowLoadingPopUp("Verifying AD...", Properties.Resources.robot1);
                System.Threading.Thread.Sleep(1000);//let pop-up thread catch up
                adAgent = new ADAgent(adIpAddressListToCheck,domainAdminUser, defaultTestPwd);
                //Determine which AD IPs are usable
                if (adAgent.adServers.Count > 0) {
                    foreach (ADServer adServer in adAgent.adServers) {
                        try {
                            bool loginSuccess = adAgent.VerifyLoginCredentialSingleSever(adServer.ipAddress, domainAdminUser, defaultTestPwd,"da");
                            if (!loginSuccess) {
                                throw new Exception();
                            }
                            atLeastOneLoginSuccess = true;
                            goodADIps.Add(adServer.ipAddress);
                            newGoodIpString.Append(adServer.ipAddress + ';');
                        } catch (Exception e) {
                            //continue
                        }
                    }
                    if (!atLeastOneLoginSuccess) {
                        Console.WriteLine("I was not able to connect to Active Directory! Check default test password and domain administrator account name! Skipping Validation for All Windows Users!");
                        MessageBox.Show(new Form { TopMost = true },"I was not able to connect to Active Directory! Check default test password and domain administrator account name! Skipping Validation for All Windows Users!",
                                    "Active Directory Connection Configuration Errors");

                    } else {
                        shipADIpAddressTextBox.Text = newGoodIpString.ToString();
                        if (goodADIps.Count > 0) {
                            foreach (DictionaryEntry pair in windowsUserTextBoxHT) {
                                TextBox currentTB = (TextBox)allTextBoxHT[pair.Key];
                                adUserToTestString = currentTB.Text.Trim();
                                adUserToTestString = adUserToTestString.Remove(0, adUserToTestString.IndexOf("\\") + 1);
                                string userType = "ra";
                                switch (currentTB.Name) {
                                    case "regularUserNameTextBox":
                                        userType = "ra";
                                        break;
                                    case "domainAdminNameTextBox":
                                        userType = "da";
                                        break;
                                    case "serverAdminNameTextBox":
                                        userType = "sa";
                                        break;
                                    case "workstationAdminNameTextBox":
                                        userType = "wa";
                                        break;
                                }
                                try {
                                    bool isValid = adAgent.VerifyLoginCredentialAllServers(adUserToTestString, defaultTestPwd,userType);
                                    Console.WriteLine("User: \'" + adUserToTestString + "\' isValid: " + isValid);
                                    if (!isValid) {
                                        failedLoginList.Add(adUserToTestString);
                                    }
                                } catch (Exception e) {
                                    message = e.Message;
                                    Console.WriteLine("I am having trouble connecting to your Active Directory(AD).\nUser:" + adUserToTestString + "\n Error: " + message + ". " +
                                        "Try checking your Active Directory IP or reset the Default Test Password.");
                                    MessageBox.Show(new Form { TopMost = true },"I am having trouble connecting to your Active Directory(AD).\nUser:" + adUserToTestString + "\n Error: " + message + ". " +
                                        "Try checking your Active Directory IP or reset the Default Test Password.",
                                        StringResources.ADConnectionErrorsMessageWindowTitle);
                                }
                            }
                        } else {
                            shipADIpAddressTextBox.Text = "No Valid IPs Given!";
                            Console.WriteLine("I was not able to connect to Active Directory! Check AD IP addresses! Skipping Validation for All Windows Users!");
                            MessageBox.Show(new Form { TopMost = true },"I was not able to connect to Active Directory! Check AD IP addresses! Skipping Validation for All Windows Users!",
                                        StringResources.ADConnectionErrorsMessageWindowTitle);
                        }

                    }
                } else {
                    shipADIpAddressTextBox.Text = "No Valid IPs Given!";
                    Console.WriteLine("I was not able to connect to Active Directory! Check AD IP addresses! Skipping Validation for All Windows Users!");
                    MessageBox.Show(new Form { TopMost = true },"I was not able to connect to Active Directory! Check AD IP addresses! Skipping Validation for All Windows Users!",
                                StringResources.ADConnectionErrorsMessageWindowTitle);
                }
                Console.WriteLine("WINDOWS ACCOUNTS COMPLETE--------------------------------->>>>>>>>>>");
                SupportFunctions.CloseLoadingPopUp();
                #endregion
                //RED HAT USER Verification--------------------------------------------------------->>>
                #region Red Hat User Verification
                Console.WriteLine("VERIFYING REDHAT ACCOUNTS---------------------------------->>>>>>>>>>");
                SupportFunctions.ShowLoadingPopUp("Verifying RedHat...", Properties.Resources.robot3);
                System.Threading.Thread.Sleep(1000);//let pop-up thread catch up

                ArrayList redHatServerArrayList = new ArrayList();
                ArrayList redHatClientArrayList = new ArrayList();

                string sshUser = linuxSSHuserNameTextBox.Text.Trim();
                string smUser = linuxSMUserNameTextBox.Text.Trim();

                if (rhServerList.Count > 0) {
                    foreach (string ip in rhServerList) {
                        redHatClientArrayList.Add(new SshClient(ip, sshUser, defaultTestPwd));
                        redHatClientArrayList.Add(new SshClient(ip, smUser, defaultTestPwd));
                    }
                    foreach (SshClient client in redHatClientArrayList) {
                        TestLogonToRedHatMachine(client, failedLoginList);
                    }
                } else {
                    Console.WriteLine("I don't have any RedHat Servers To Check!!");
                }
                Console.WriteLine("REDHAT ACCOUNTS COMPLETE---------------------------------->>>>>>>>>>");
                SupportFunctions.CloseLoadingPopUp();
                #endregion
                //ESX USER Verification--------------------------------------------------------->>>>
                #region ESX User Verification
                Console.WriteLine("VERIFYING ESX ACCOUNTS------------------------------------->>>>>>>>>>");
                SupportFunctions.ShowLoadingPopUp("Verifying ESX...", Properties.Resources.robot2);
                System.Threading.Thread.Sleep(1000);//let pop-up thread catch up
                string rtUser = esxUserNameTextBox.Text.Trim();
                if (esxAgent == null) { esxAgent = new EsxAgent(rtUser, esxIpList); }
                if (!TestLogonToESXHost(failedLoginList, rtUser)) {
                    esxUserNameTextBox.Text = (esxUserNameTextBox.Text + "<-Check IPs!");
                }
                SupportFunctions.CloseLoadingPopUp();
                Console.WriteLine("ESX ACCOUNTS COMPLETE------------------------------------->>>>>>>>>>");
                #endregion
                //If successful, save info, if not inform user and mark bad lines with "->Not Valid!!"
                SupportFunctions.ShowLoadingPopUp("Compiling Results...", Properties.Resources.robot4);
                System.Threading.Thread.Sleep(1000);//let pop-up thread catch up
                if (failedLoginList.Count > 0) {
                    StringBuilder badAccountsSB = new StringBuilder();
                    StringBuilder badIpsSB = new StringBuilder();
                    foreach (string failedLogin in failedLoginList) {
                        if (failedLogin.StartsWith("IP Address:")) {
                            if (!badIpsSB.ToString().Contains(failedLogin)) {
                                badIpsSB.Append("!>" + failedLogin + "\n");
                            }
                        } else {
                            badAccountsSB.Append("!>" + failedLogin + "\n");
                        }
                        foreach (DictionaryEntry pair in allTextBoxHT) {
                            TextBox currentTB = (TextBox)allTextBoxHT[pair.Key];
                            //This must be done both ways because failedLogin syntax is different for different server types
                            if ((currentTB.Text.Contains(failedLogin) || failedLogin.Contains(currentTB.Text)) && !currentTB.Text.Equals(locUserNameTextBox.Text)) {
                                currentTB.Text = currentTB.Text + "->Not VALID!";
                            }
                        }
                    }
                    String cummulativeErrorString = "Some accounts were incorrect.\n\nThe password could be different, in AD or locally." +
                        "You could have incorrectly typed the account name, password or IP address for that server.  If it " +
                        "is a local account for an ESX host or RedHat machine, you may want to ensure the local password isn't expired." +
                        "Please fix them, current settings have not been saved. \n\nI have added \"->Not VALID!\" or \"->Check IPs!\" to the end of each incorrect lines." +
                        "You will need to remove this before saving.\n\n" +
                        "If you wish to save these accounts please hit the save button.\n\n";
                    //If there were incorrect IPs, append them to error message
                    if (badAccountsSB.Length > 0) {
                        cummulativeErrorString = (cummulativeErrorString + "These accounts had issues:\n\n" + badAccountsSB.ToString());
                    }
                    //If there were incorrect IPs, append them to error message
                    if (badIpsSB.Length > 0) {
                        cummulativeErrorString = (cummulativeErrorString + "These IPs weren't valid, check the IPs that were set in Server Settings Menu:\n\n" + badIpsSB.ToString());
                    }
                    MessageBox.Show(new Form { TopMost = true }, cummulativeErrorString, accountValidatorMessageWindowTitle);
                } else {
                    MessageBox.Show(new Form { TopMost = true }, "I have verified all accounts. I will now save configuration. Thanks for being great.", accountValidatorMessageWindowTitle);
                    saveUserSettingsBtn.PerformClick();
                }
                SupportFunctions.CloseLoadingPopUp();
            };
            //Calls password change form for changing local account password
            changeLocalPassBtn.Click += delegate {
                ChangeLocalPasswordForm changeLocalPasswordWindow = new ChangeLocalPasswordForm();
                changeLocalPasswordWindow.ShowDialog();
            };
            //Sets Hull variable for each username when hull text value is changed
            shipHullNumberTextBox.TextChanged += delegate {
                RefreshUserListHulls(windowsUserTextBoxHT, shipHullNumberTextBox);
            };
            //Checks Hull on users when formInputWindowTableLayoutPanel is clicked
            formInputWindowTableLayoutPanel.Click += delegate {
                RefreshUserListHulls(windowsUserTextBoxHT, shipHullNumberTextBox);
            };
            //Checks Hull on users when mouse is moved inside formInputWindowTableLayoutPanel
            formInputWindowTableLayoutPanel.MouseMove += delegate {
                RefreshUserListHulls(windowsUserTextBoxHT, shipHullNumberTextBox);
            };
            #endregion        
            return validateUserSettingsBtn;
        }
        private Button BuildWindowsAndLocalSettingsInterface() {
            //Interface Control builders------------------------------------------------------------------------------------>>>
            #region Interface Control Declarations and Settings
            Hashtable allTextBoxHT = new Hashtable();
            Hashtable windowsUserTextBoxHT = new Hashtable();

            Label shipSettingsTitleLabel = new Label();
            shipSettingsTitleLabel.Text = "Ship Settings";
            shipSettingsTitleLabel.Font = new Font("Comic Sans MS", 12);
            shipSettingsTitleLabel.AutoSize = true;
            shipSettingsTitleLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Label shipHullNumberLabel = new Label();
            shipHullNumberLabel.Text = "Ship Hull Number(ex:ncc1031):";
            shipHullNumberLabel.AutoSize = true;
            shipHullNumberLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox shipHullNumberTextBox = new TextBox();
            shipHullNumberTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(hullNumber);
            shipHullNumberTextBox.AutoSize = false;
            shipHullNumberTextBox.Width = 300;
            shipHullNumberTextBox.Anchor = AnchorStyles.Left;
            shipHullNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("shipHullNumberTextBox", shipHullNumberTextBox);

            Label shipADIpAddressLabel = new Label();
            shipADIpAddressLabel.Text = "Active Directory IP Addresses:";
            shipADIpAddressLabel.AutoSize = true;
            shipADIpAddressLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox shipADIpAddressTextBox = new TextBox();
            StringBuilder sb = new StringBuilder();
            foreach (string ip in adIpAddressList) {
                if (!ip.Equals("")) {
                    sb.Append(ip + ";");
                };
            }
            shipADIpAddressTextBox.Text = sb.ToString();
            shipADIpAddressTextBox.AutoSize = false;
            shipADIpAddressTextBox.Width = 300;
            shipADIpAddressTextBox.Anchor = AnchorStyles.Left;
            shipADIpAddressTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("shipADIpAddressTextBox", shipADIpAddressTextBox);

            Label userSettingsTitleLabel = new Label();
            userSettingsTitleLabel.Text = "User Settings";
            userSettingsTitleLabel.Font = new Font("Comic Sans MS", 12);
            userSettingsTitleLabel.AutoSize = true;
            userSettingsTitleLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Label regularUserNameLabel = new Label();
            regularUserNameLabel.Text = "Regular User Name(ex:<hull#>\\<username>):";
            regularUserNameLabel.AutoSize = true;
            regularUserNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox regularUserNameTextBox = new TextBox();
            regularUserNameTextBox.Name = "regularUserNameTextBox";
            regularUserNameTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(regUserName);
            regularUserNameTextBox.Text = (hullNumber + "\\" + regularUserNameTextBox.Text);
            regularUserNameTextBox.AutoSize = false;
            regularUserNameTextBox.Width = 300;
            regularUserNameTextBox.Anchor = AnchorStyles.Left;
            regularUserNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("regularUserNameTextBox", regularUserNameTextBox);
            windowsUserTextBoxHT.Add("regularUserNameTextBox", regularUserNameTextBox);

            Label domainAdminNameLabel = new Label();
            domainAdminNameLabel.Text = "Domain Admin Name(ex:<hull#>\\<username>):";
            domainAdminNameLabel.AutoSize = true;
            domainAdminNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox domainAdminNameTextBox = new TextBox();
            domainAdminNameTextBox.Name = "domainAdminNameTextBox";
            domainAdminNameTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(domainAdminName);
            domainAdminNameTextBox.Text = (hullNumber + "\\" + domainAdminNameTextBox.Text);
            domainAdminNameTextBox.AutoSize = false;
            domainAdminNameTextBox.Width = 300;
            domainAdminNameTextBox.Anchor = AnchorStyles.Left;
            domainAdminNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("domainAdminNameTextBox", domainAdminNameTextBox);
            windowsUserTextBoxHT.Add("domainAdminNameTextBox", domainAdminNameTextBox);

            Label serverAdminNameLabel = new Label();
            serverAdminNameLabel.Text = "Server Admin Name(ex:<hull#>\\<username>):";
            serverAdminNameLabel.AutoSize = true;
            serverAdminNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox serverAdminNameTextBox = new TextBox();
            serverAdminNameTextBox.Name = "serverAdminNameTextBox";
            serverAdminNameTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(serverAdminName);
            serverAdminNameTextBox.Text = (hullNumber + "\\" + serverAdminNameTextBox.Text);
            serverAdminNameTextBox.AutoSize = false;
            serverAdminNameTextBox.Width = 300;
            serverAdminNameTextBox.Anchor = AnchorStyles.Left;
            serverAdminNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("serverAdminNameTextBox", serverAdminNameTextBox);
            windowsUserTextBoxHT.Add("serverAdminNameTextBox", serverAdminNameTextBox);

            Label workstationAdminNameLabel = new Label();
            workstationAdminNameLabel.Text = "Workstation Admin Name(ex:<hull#>\\<username>):";
            workstationAdminNameLabel.AutoSize = true;
            workstationAdminNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            TextBox workstationAdminNameTextBox = new TextBox();
            workstationAdminNameTextBox.Name = "workstationAdminNameTextBox";
            workstationAdminNameTextBox.Text = SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(workstationAdminName);
            workstationAdminNameTextBox.Text = (hullNumber + "\\" + workstationAdminNameTextBox.Text);
            workstationAdminNameTextBox.AutoSize = false;
            workstationAdminNameTextBox.Width = 300;
            workstationAdminNameTextBox.Anchor = AnchorStyles.Left;
            workstationAdminNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            allTextBoxHT.Add("workstationAdminNameTextBox", workstationAdminNameTextBox);
            windowsUserTextBoxHT.Add("workstationAdminNameTextBox", workstationAdminNameTextBox);

            TableLayoutPanel localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel = new TableLayoutPanel();
            localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel.AutoSize = true;

            Label localUserNameLabel = new Label();
            localUserNameLabel.Text = "Local User Name:";
            localUserNameLabel.AutoSize = true;
            localUserNameLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Button changeLocalPassBtn = new Button();
            changeLocalPassBtn.Text = "Change Local Password";
            changeLocalPassBtn.AutoSize = true;
            changeLocalPassBtn.Font = new Font(changeLocalPassBtn.Font.FontFamily, 7);
            changeLocalPassBtn.Margin = new Padding(0, 0, 0, 0);

            TextBox locUserNameTextBox = new TextBox();
            locUserNameTextBox.Text = localUserName;
            locUserNameTextBox.AutoSize = false;
            locUserNameTextBox.Width = 300;
            locUserNameTextBox.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            locUserNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            locUserNameTextBox.Margin = new Padding(0, 2, 0, 0);
            allTextBoxHT.Add("locUserNameTextBox", locUserNameTextBox);

            TableLayoutPanel userSettingsButtonTableLayoutPanel = new TableLayoutPanel();
            userSettingsButtonTableLayoutPanel.AutoSize = true;

            Button saveUserSettingsBtn = new Button();
            saveUserSettingsBtn.Text = "Save User Settings";
            saveUserSettingsBtn.AutoSize = true;

            Button validateUserSettingsBtn = new Button();
            validateUserSettingsBtn.Text = "Validate and Save User Settings";
            validateUserSettingsBtn.AutoSize = true;

            int counter = 0;
            localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel.Controls.Add(locUserNameTextBox, 0, 0);
            localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel.Controls.Add(changeLocalPassBtn, 1, 0);
            userSettingsButtonTableLayoutPanel.Controls.Add(saveUserSettingsBtn, 0, 0);
            userSettingsButtonTableLayoutPanel.Controls.Add(validateUserSettingsBtn, 1, 0);
            formInputWindowTableLayoutPanel.Controls.Add(shipSettingsTitleLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(shipHullNumberLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(shipHullNumberTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(shipADIpAddressLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(shipADIpAddressTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(userSettingsTitleLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(regularUserNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(regularUserNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(domainAdminNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(domainAdminNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(serverAdminNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(serverAdminNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(workstationAdminNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(workstationAdminNameTextBox, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(localUserNameLabel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(localUserSettingsLabelAndChangePasswordButtonTableLayoutPanel, 0, counter++);
            formInputWindowTableLayoutPanel.Controls.Add(userSettingsButtonTableLayoutPanel, 0, counter++);
            #endregion

            //EVENT HANDLERS------------------------------------------------------------------------------------>>>
            #region Event Handlers
            //checks user input for mistakes then save to configuration
            saveUserSettingsBtn.Click += delegate {
                RefreshUserListHulls(windowsUserTextBoxHT, shipHullNumberTextBox);
                CheckSettingsFiles();

                ArrayList newValuesToSave = new ArrayList();
                ArrayList errorList = new ArrayList();

                foreach (DictionaryEntry pair in allTextBoxHT) {
                    TextBox currentTB = (TextBox)allTextBoxHT[pair.Key];
                    switch (pair.Key) {
                        case "shipHullNumberTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestHullRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Ship Hull: You can only use numbers and letters for the hull!");
                            } else {
                                newValuesToSave.Add("[HULL]" + currentTB.Text.Trim());
                            }
                            break;
                        case "shipADIpAddressTextBox":
                            string line = currentTB.Text.Replace("[ADIP]", "");
                            ArrayList adIpAddressListToCheck = new ArrayList(line.Split(';'));
                            StringBuilder adIpAddressListChecked = new StringBuilder();
                            adIpAddressListChecked.Append("[ADIP]");
                            foreach (string ip in adIpAddressListToCheck) {
                                if (!ip.Equals("")) {
                                    if (!(SQTSupportFunctions.SupportFunctions.ValidateIp(ip))) {
                                        errorList.Add("Ship AD: Value entered (" + ip + ") is not an IP!");
                                    } else {
                                        adIpAddressListChecked.Append(ip + ";");
                                    }
                                }
                            }
                            newValuesToSave.Add(adIpAddressListChecked.ToString());
                            break;
                        case "regularUserNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Regular User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            } else {
                                newValuesToSave.Add("[USER][REG]" + currentTB.Text.Trim());
                            }
                            break;
                        case "domainAdminNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Domain Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            } else {
                                newValuesToSave.Add("[USER][DA]" + currentTB.Text.Trim());
                            }
                            break;
                        case "serverAdminNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Server Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            } else {
                                newValuesToSave.Add("[USER][SA]" + currentTB.Text.Trim());
                            }
                            break;
                        case "workstationAdminNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Workstation Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            } else {
                                newValuesToSave.Add("[USER][WA]" + currentTB.Text.Trim());
                            }
                            break;
                        case "locUserNameTextBox":
                            if (!(SQTSupportFunctions.SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                                errorList.Add("Local User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods");
                            } else {
                                newValuesToSave.Add("[USER][LOC]" + currentTB.Text.Trim());
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
                    MessageBox.Show(new Form { TopMost = true },"I need you to fix these issues:\n" + errorListSB.ToString(), hullandUserValidatorMessageWindowTitle);
                    return;
                } else {
                    //Check for save file
                    if (!File.Exists(configurationFilePath)) {
                        File.Create(configurationFilePath).Close();
                    }
                    //Read file and get specific line to replace
                    StreamReader configFileReader = new StreamReader(configurationFilePath);
                    configurationFilePath = Path.GetFullPath(configurationFilePath);
                    Console.WriteLine("\nConfig File Location: " + configurationFilePath);

                    string line;//holds read line to be read, identifies line by [xxx] value, then passes to appropriate variable for use later
                    StringBuilder newConfigLineSB = new StringBuilder();//builds new line to save

                    //Read file to get values values that aren't changed on this screen from config file
                    while ((line = configFileReader.ReadLine()) != null) {
                        //Save values that can't be modified here
                        if (line.StartsWith("[ESXIP]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                        if (line.StartsWith("[WEBTEST][EXTERNAL]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                        if (line.StartsWith("[WEBTEST][INTERNAL]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                        if (line.StartsWith("[LOCPH]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                        if (line.StartsWith("[REDHAT]")) {
                            newValuesToSave.Add(line);
                            continue;
                        }; 
                        if (line.StartsWith("[USER][SM]")) {
                            newValuesToSave.Add(line);
                            continue;
                        }; 
                        if (line.StartsWith("[USER][SSH]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                        if (line.StartsWith("[USER][ESX]")) {
                            newValuesToSave.Add(line);
                            continue;
                        };
                    }
                    configFileReader.Close();

                    //Replace old config line and set text file contents
                    newValuesToSave.Sort();
                    StringBuilder configFileTextBuilder = new StringBuilder();
                    for (int i = 0; i < newValuesToSave.Count; i++) {
                        configFileTextBuilder.Append(newValuesToSave[i].ToString() + "\n");
                        Console.WriteLine("Value To Saved: " + newValuesToSave[i].ToString());
                    }
                    try {
                        File.WriteAllText(configurationFilePath, configFileTextBuilder.ToString().Trim());
                    } catch (Exception e) {
                        MessageBox.Show(new Form { TopMost = true },"There was an error trying to write to your config file!\nPlease check the file isn't being used by another program and retry save action.\n" + e.Message, ipAddressValidatorMessageWindowTitle);
                    }
                }
                formInputWindowTableLayoutPanel.Controls.Clear();
                CheckSettingsFiles();
                BuildWindowsAndLocalSettingsInterface();
            };
            //validates Windows Users
            validateUserSettingsBtn.Click += delegate {
                MessageBox.Show(new Form { TopMost = true },"I will now save settings, remove invalid AD Addresses, and begin checking your accounts.", accountValidatorMessageWindowTitle);
                Console.WriteLine("I will now save settings, remove invalid AD Addresses, and begin check Windows Machines");
                ArrayList goodADIps = new ArrayList();
                ArrayList adIpAddressListToCheck = new ArrayList();
                ArrayList failedLoginList = new ArrayList();
                string message = "DEFAULT ERROR";
                string adUserToTestString = "TEXTBOXNULL";
                string domainAdminUser = domainAdminNameTextBox.Text.Remove(0, domainAdminNameTextBox.Text.IndexOf("\\") + 1);
                bool atLeastOneLoginSuccess = false;
                //capture and check current input box values, abort operation if one is wrong
                if (!ValidateUserAccountInput(allTextBoxHT)) {
                    return;
                };
                //WINDOWS USER Verification--------------------------------------------------------->>>
                #region Windows Verification
                Console.WriteLine("VERIFYING WINDOWS ACCOUNTS--------------------------------->>>>>>>>>>");
                //get new IP list to check
                adIpAddressListToCheck = new ArrayList(shipADIpAddressTextBox.Text.Trim().Split(';'));
                StringBuilder newGoodIpString = new StringBuilder();
                bool failedToConnectToAD = false;
                //reload agent
                SupportFunctions.ShowLoadingPopUp("Verifying AD...", Properties.Resources.robot1);
                System.Threading.Thread.Sleep(1000);//let pop-up thread catch up
                Console.WriteLine("Creating Windows agent....");
                adAgent = new ADAgent(adIpAddressListToCheck, domainAdminUser, defaultTestPwd);
                //Determine which AD IPs are usable
                if (adAgent.adServers.Count > 0) {
                    foreach (ADServer adServer in adAgent.adServers) {
                        try {
                            bool loginSuccess = adAgent.VerifyLoginCredentialSingleSever(adServer.ipAddress, domainAdminUser, defaultTestPwd,"da");
                            if (!loginSuccess) {
                                throw new Exception();
                            }
                            atLeastOneLoginSuccess = true;
                            goodADIps.Add(adServer.ipAddress);
                            newGoodIpString.Append(adServer.ipAddress + ';');
                        } catch (Exception e) {
                            //continue
                        }
                    }
                    if (!atLeastOneLoginSuccess) {
                        Console.WriteLine("I was not able to connect to Active Directory! Check default test password and domain administrator account name! Skipping Validation for All Windows Users!");
                        MessageBox.Show(new Form { TopMost = true },"I was not able to connect to Active Directory! Check default test password and domain administrator account name! Skipping Validation for All Windows Users!",
                                    "Active Directory Connection Configuration Errors");
                        failedToConnectToAD = true;
                    } else {
                        Console.WriteLine("Testing Windows Accounts....");
                        shipADIpAddressTextBox.Text = newGoodIpString.ToString();
                        if (goodADIps.Count > 0) {
                            foreach (DictionaryEntry pair in windowsUserTextBoxHT) {
                                TextBox currentTB = (TextBox)allTextBoxHT[pair.Key];
                                adUserToTestString = currentTB.Text.Trim();
                                adUserToTestString = adUserToTestString.Remove(0, adUserToTestString.IndexOf("\\") + 1);
                                string userType = "ra";
                                switch (currentTB.Name) {
                                    case "regularUserNameTextBox":
                                        userType = "ra";
                                        break;
                                    case "domainAdminNameTextBox":
                                        userType = "da";
                                        break;
                                    case "serverAdminNameTextBox":
                                        userType = "sa";
                                        break;
                                    case "workstationAdminNameTextBox":
                                        userType = "wa";
                                        break;
                                }
                                try {
                                    bool isValid = adAgent.VerifyLoginCredentialAllServers(adUserToTestString, defaultTestPwd, userType);
                                    Console.WriteLine("User: \'" + adUserToTestString + "\' isValid: " + isValid);
                                    if (!isValid) {
                                        failedLoginList.Add(adUserToTestString+"<Invalid Group Memebership");
                                        currentTB.Text = currentTB.Text + "<-INVALID AD GROUP";
                                    }
                                } catch (NullReferenceException e) {
                                    message = e.Message;
                                    Console.WriteLine("User: \'" + adUserToTestString + "\' isValid: false");
                                    failedLoginList.Add(adUserToTestString + "<Invalid Username or Password");
                                    currentTB.Text = currentTB.Text + "<-INVALID USER/PWD";

                                } catch (Exception ex) {
                                    message = ex.Message;
                                    failedLoginList.Add(adUserToTestString + "<Unknown Error!!!");
                                    currentTB.Text = currentTB.Text + "<-UNKNOWN ERROR";
                                    Console.WriteLine("User: \'" + adUserToTestString + "\' isValid: false");
                                    Console.WriteLine("I am having trouble connecting to your Active Directory(AD).\nUser:" + adUserToTestString + "\nError: " + message + ". " +
                                        "Try checking your Active Directory IP or reset the Default Test Password.");
                                }
                            }
                        } else {
                            shipADIpAddressTextBox.Text = "No Valid IPs Given!";
                            Console.WriteLine("I was not able to connect to Active Directory! Check AD IP addresses! Skipping Validation for All Windows Users!");
                            MessageBox.Show(new Form { TopMost = true },"I was not able to connect to Active Directory! Check AD IP addresses! Skipping Validation for All Windows Users!",
                                        StringResources.ADConnectionErrorsMessageWindowTitle);
                            failedToConnectToAD = true;
                        }
                        
                    }
                } else {
                    shipADIpAddressTextBox.Text = "No Valid IPs Given!";
                    Console.WriteLine("I was not able to connect to Active Directory! Check AD IP addresses! Skipping Validation for All Windows Users!");
                    MessageBox.Show(new Form { TopMost = true },"I was not able to connect to Active Directory! Check AD IP addresses! Skipping Validation for All Windows Users!",
                                StringResources.ADConnectionErrorsMessageWindowTitle);
                    failedToConnectToAD = true;
                }
                Console.WriteLine("WINDOWS ACCOUNTS COMPLETE--------------------------------->>>>>>>>>>");
                SupportFunctions.CloseLoadingPopUp();
                if (failedToConnectToAD) { return; }
                #endregion
                //If successful, save info, if not inform user and mark bad lines with "->Not Valid!!"
                SupportFunctions.ShowLoadingPopUp("Compiling Results...", Properties.Resources.robot4);
                System.Threading.Thread.Sleep(1000);//let pop-up thread catch up
                if (failedLoginList.Count > 0) {
                    StringBuilder badAccountsSB = new StringBuilder();
                    StringBuilder badIpsSB = new StringBuilder();
                    foreach (string failedLogin in failedLoginList) {
                        if (failedLogin.StartsWith("IP Address:")) {
                            if (!badIpsSB.ToString().Contains(failedLogin)) {
                                badIpsSB.Append("!>" + failedLogin + "\n");
                            }
                        } else {
                            badAccountsSB.Append("!>" + failedLogin + "\n");
                        }
                    }
                    String cummulativeErrorString = "Some accounts were incorrect.\n\nThe password could be different, in AD or locally." +
                        "You could have incorrectly typed the account name, password or IP address for that server.  If it " +
                        "is a local account for an ESX host or RedHat machine, you may want to ensure the local password isn't expired." +
                        "Please fix them, current settings have not been saved. \n\nI have added \"->Not VALID!\" or to the end of each incorrect lines." +
                        "You will need to remove this before saving.\n\n" +
                        "If you wish to save these accounts please hit the save button.\n\n";
                    //If there were incorrect IPs, append them to error message
                    if (badAccountsSB.Length > 0) {
                        cummulativeErrorString = (cummulativeErrorString + "These accounts had issues:\n\n" + badAccountsSB.ToString());
                    }
                    //If there were incorrect IPs, append them to error message
                    if (badIpsSB.Length > 0) {
                        cummulativeErrorString = (cummulativeErrorString + "These IPs weren't valid, check the IPs that were set in Server Settings Menu:\n\n" + badIpsSB.ToString());
                    }
                    MessageBox.Show(new Form { TopMost = true }, cummulativeErrorString, accountValidatorMessageWindowTitle);
                } else {
                    MessageBox.Show(new Form { TopMost = true }, "I have verified all accounts. I will now save configuration. Thanks for being great.", accountValidatorMessageWindowTitle);
                    saveUserSettingsBtn.PerformClick();
                }
                SupportFunctions.CloseLoadingPopUp();
            };
            //Calls password change form for changing local account password
            changeLocalPassBtn.Click += delegate {
                ChangeLocalPasswordForm changeLocalPasswordWindow = new ChangeLocalPasswordForm();
                changeLocalPasswordWindow.ShowDialog();
            };
            //Sets Hull variable for each username when hull text value is changed
            shipHullNumberTextBox.TextChanged += delegate {
                RefreshUserListHulls(windowsUserTextBoxHT, shipHullNumberTextBox);
            };
            //Checks Hull on users when formInputWindowTableLayoutPanel is clicked
            formInputWindowTableLayoutPanel.Click += delegate {
                RefreshUserListHulls(windowsUserTextBoxHT, shipHullNumberTextBox);
            };
            //Checks Hull on users when mouse is moved inside formInputWindowTableLayoutPanel
            formInputWindowTableLayoutPanel.MouseMove += delegate {
                RefreshUserListHulls(windowsUserTextBoxHT, shipHullNumberTextBox);
            };
            #endregion        
            return validateUserSettingsBtn;
        }
        private void BuildWebsitesSettingsInterface() {

            //Interface Control builders------------------------------------------------------------------------------------>>>
            #region Interface Control Declarations and Settings

            Label websiteSettingsLabelTitleLabel = new Label();
            websiteSettingsLabelTitleLabel.Text = "SQT Website Settings";
            websiteSettingsLabelTitleLabel.Font = new Font("Comic Sans MS", 12);
            websiteSettingsLabelTitleLabel.AutoSize = true;
            websiteSettingsLabelTitleLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Label externalWebsiteSiteListLabel = new Label();
            externalWebsiteSiteListLabel.Text = "External Website List:";
            externalWebsiteSiteListLabel.AutoSize = true;
            externalWebsiteSiteListLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            ListView externalSitesToTestListView = new ListView();
            ColumnHeader ch1 = new ColumnHeader();
            ch1.Text = "";
            ch1.Name = "col";
            ch1.Width = 240;
            externalSitesToTestListView.Columns.Add(ch1);
            externalSitesToTestListView.HeaderStyle = ColumnHeaderStyle.None;
            externalSitesToTestListView.CheckBoxes = true;
            externalSitesToTestListView.Scrollable = true;
            externalSitesToTestListView.View = View.Details;
            externalSitesToTestListView.Width = 300;
            foreach (String s in externalSiteList) {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = s;
                externalSitesToTestListView.Items.Add(lvi);
            }

            Button selectAllExternalSiteBtn = new Button();
            selectAllExternalSiteBtn.Text = "Select All";
            selectAllExternalSiteBtn.AutoSize = true;

            Button clearAllExternalSiteBtn = new Button();
            clearAllExternalSiteBtn.Text = "Clear All";
            clearAllExternalSiteBtn.AutoSize = true;

            Button addExternalSiteBtn = new Button();
            addExternalSiteBtn.Text = "Add Site";
            addExternalSiteBtn.AutoSize = true;

            Button removeSelectedExternalSitesBtn = new Button();
            removeSelectedExternalSitesBtn.Text = "Remove Selected";
            removeSelectedExternalSitesBtn.AutoSize = true;

            TableLayoutPanel externalSiteListButtonTableLayoutPanel = new TableLayoutPanel();
            externalSiteListButtonTableLayoutPanel.AutoSize = true;
            externalSiteListButtonTableLayoutPanel.Controls.Add(selectAllExternalSiteBtn, 0, 0);
            externalSiteListButtonTableLayoutPanel.Controls.Add(clearAllExternalSiteBtn, 1, 0);
            externalSiteListButtonTableLayoutPanel.Controls.Add(addExternalSiteBtn, 2, 0);
            externalSiteListButtonTableLayoutPanel.Controls.Add(removeSelectedExternalSitesBtn, 3, 0);

            Label internalWebsiteSiteListLabel = new Label();
            internalWebsiteSiteListLabel.Text = "Internal Website List:";
            internalWebsiteSiteListLabel.AutoSize = true;
            internalWebsiteSiteListLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            ListView internalSitesToTestListView = new ListView();
            ColumnHeader ch2 = new ColumnHeader();
            ch2.Text = "";
            ch2.Name = "col";
            ch2.Width = 240;
            internalSitesToTestListView.Columns.Add(ch2);
            internalSitesToTestListView.HeaderStyle = ColumnHeaderStyle.None;
            internalSitesToTestListView.CheckBoxes = true;
            internalSitesToTestListView.Scrollable = true;
            internalSitesToTestListView.View = View.Details;
            internalSitesToTestListView.Width = 300;
            foreach (String s in internalSiteList) {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = s;
                internalSitesToTestListView.Items.Add(lvi);
            }

            Button selectAllInternalSiteBtn = new Button();
            selectAllInternalSiteBtn.Text = "Select All";
            selectAllInternalSiteBtn.AutoSize = true;

            Button clearAllInternalSiteBtn = new Button();
            clearAllInternalSiteBtn.Text = "Clear All";
            clearAllInternalSiteBtn.AutoSize = true;

            Button addInternalSiteBtn = new Button();
            addInternalSiteBtn.Text = "Add Site";
            addInternalSiteBtn.AutoSize = true;

            Button removeSelectedInternalSitesBtn = new Button();
            removeSelectedInternalSitesBtn.Text = "Remove Selected";
            removeSelectedInternalSitesBtn.AutoSize = true;

            TableLayoutPanel internalSiteListButtonTableLayoutPanel = new TableLayoutPanel();
            internalSiteListButtonTableLayoutPanel.AutoSize = true;
            internalSiteListButtonTableLayoutPanel.Controls.Add(selectAllInternalSiteBtn, 0, 0);
            internalSiteListButtonTableLayoutPanel.Controls.Add(clearAllInternalSiteBtn, 1, 0);
            internalSiteListButtonTableLayoutPanel.Controls.Add(addInternalSiteBtn, 2, 0);
            internalSiteListButtonTableLayoutPanel.Controls.Add(removeSelectedInternalSitesBtn, 3, 0);

            formInputWindowTableLayoutPanel.Controls.Add(websiteSettingsLabelTitleLabel, 0, 0);
            formInputWindowTableLayoutPanel.Controls.Add(externalWebsiteSiteListLabel, 0, 1);
            formInputWindowTableLayoutPanel.Controls.Add(externalSitesToTestListView, 0, 2);
            formInputWindowTableLayoutPanel.Controls.Add(externalSiteListButtonTableLayoutPanel, 0, 3);
            formInputWindowTableLayoutPanel.Controls.Add(internalWebsiteSiteListLabel, 0, 4);
            formInputWindowTableLayoutPanel.Controls.Add(internalSitesToTestListView, 0, 5);
            formInputWindowTableLayoutPanel.Controls.Add(internalSiteListButtonTableLayoutPanel, 0, 6);
            formInputWindowTableLayoutPanel.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            #endregion
            //Event Handlers------------------------------------------------------------------------------------>>>
            #region Event Handlers
            selectAllExternalSiteBtn.Click += delegate { foreach (ListViewItem item in externalSitesToTestListView.Items) { item.Checked = true; externalSitesToTestListView.Refresh(); } };
            clearAllExternalSiteBtn.Click += delegate { foreach (ListViewItem item in externalSitesToTestListView.Items) { item.Checked = false; externalSitesToTestListView.Refresh(); } };
            addExternalSiteBtn.Click += delegate {
                string newSite = Interaction.InputBox("Please enter the the new website you would like to add: ", websiteManagerMessageWindowTitle);
                if (externalSiteList.Contains(newSite)) {//check for duplicates
                    MessageBox.Show(new Form { TopMost = true },newSite + " has already been added to this list.", StringResources.websiteManagerMessageWindowTitle);
                } else {
                    if (SupportFunctions.TestWebsiteString(newSite, true)) {
                        SupportFunctions.AddListViewItemAndRefresh(externalSitesToTestListView, externalSiteList, newSite);
                    }
                }
                SaveWebsitesLists(externalSitesToTestListView, internalSitesToTestListView);
            };
            removeSelectedExternalSitesBtn.Click += delegate {
                ArrayList itemsToRemove = new ArrayList(externalSitesToTestListView.CheckedItems);
                StringBuilder removeSitesMessage = new StringBuilder();
                removeSitesMessage.Append("I will now remove the following sites:\n\n");
                foreach (ListViewItem item in itemsToRemove) {
                    removeSitesMessage.Append(">" + item.Text + "\n");
                }
                DialogResult userAnswer = MessageBox.Show(new Form { TopMost = true },removeSitesMessage.ToString(), StringResources.websiteManagerMessageWindowTitle, MessageBoxButtons.OKCancel);
                if (userAnswer == DialogResult.OK) {
                    foreach (ListViewItem item in itemsToRemove) {
                        SupportFunctions.removeListViewItemAndRefresh(externalSitesToTestListView, externalSiteList, item);
                    }
                }
                SaveWebsitesLists(externalSitesToTestListView, internalSitesToTestListView);
            };
            selectAllInternalSiteBtn.Click += delegate { foreach (ListViewItem item in internalSitesToTestListView.Items) { item.Checked = true; internalSitesToTestListView.Refresh(); } };
            clearAllInternalSiteBtn.Click += delegate { foreach (ListViewItem item in internalSitesToTestListView.Items) { item.Checked = false; internalSitesToTestListView.Refresh(); } };
            addInternalSiteBtn.Click += delegate {
                string newSite = Interaction.InputBox("Please enter the the new website you would like to add: ", websiteManagerMessageWindowTitle);
                if (internalSiteList.Contains(newSite)) {//check for duplicates
                    MessageBox.Show(new Form { TopMost = true },newSite + " has already been added to this list.", StringResources.websiteManagerMessageWindowTitle);
                } else {
                    if (SupportFunctions.TestWebsiteString(newSite, true)) {
                        SupportFunctions.AddListViewItemAndRefresh(internalSitesToTestListView, internalSiteList, newSite);
                    }
                }
                SaveWebsitesLists(externalSitesToTestListView, internalSitesToTestListView);
            };
            removeSelectedInternalSitesBtn.Click += delegate {
                ArrayList itemsToRemove = new ArrayList(internalSitesToTestListView.CheckedItems);
                StringBuilder removeSitesMessage = new StringBuilder();
                removeSitesMessage.Append("I will now remove the following sites:\n\n");
                foreach (ListViewItem item in itemsToRemove) {
                    removeSitesMessage.Append(">" + item.Text + "\n");
                }
                DialogResult userAnswer = MessageBox.Show(new Form { TopMost = true },removeSitesMessage.ToString(), StringResources.websiteManagerMessageWindowTitle, MessageBoxButtons.OKCancel);
                if (userAnswer == DialogResult.OK) {
                    foreach (ListViewItem item in itemsToRemove) {
                        SupportFunctions.removeListViewItemAndRefresh(internalSitesToTestListView, internalSiteList, item);
                    }
                }
                SaveWebsitesLists(externalSitesToTestListView, internalSitesToTestListView);
            };
            #endregion
        }
        private void BuildDeckProfileSettingsInterface() {
            #region Build interface
            Label DeckProfileSettingsTitleLabel = new Label();
            DeckProfileSettingsTitleLabel.Text = "Import/Export Settings\n\n";
            DeckProfileSettingsTitleLabel.AutoSize = true;
            DeckProfileSettingsTitleLabel.Font = new Font("Comic Sans MS", 12);
            DeckProfileSettingsTitleLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Label DeckProfileSettingsHullTitleLabel = new Label();
            DeckProfileSettingsHullTitleLabel.Text = "Current System Profile Hull Number:";
            DeckProfileSettingsHullTitleLabel.AutoSize = true;

            TextBox DeckProfileSettingsHullTextBox = new TextBox();
            if (hullNumber.Equals("")) {
                DeckProfileSettingsHullTextBox.Text = "<No Hull Loaded>";
            } else {
                DeckProfileSettingsHullTextBox.Text = hullNumber;
            }
            DeckProfileSettingsHullTextBox.Width = 400;
            DeckProfileSettingsHullTextBox.Enabled = false;

            Label DeckProfileSettingsFilePathLabel = new Label();
            DeckProfileSettingsFilePathLabel.Text = "File Path to Settings File:";
            DeckProfileSettingsFilePathLabel.AutoSize = true;

            TextBox DeckProfileSettingsFilePathTextBox = new TextBox();
            DeckProfileSettingsFilePathTextBox.Text = (Path.GetFullPath(".\\") + "SSAManager.conf");
            DeckProfileSettingsFilePathTextBox.Width = 400;
            DeckProfileSettingsFilePathTextBox.Enabled = false;

            Button importDeckProfileBtn = new Button();
            importDeckProfileBtn.Text = "Import Deck Profile";
            importDeckProfileBtn.AutoSize = true;

            Button importDeckProfileBrowseBtn = new Button();
            importDeckProfileBrowseBtn.Text = "Browse";
            importDeckProfileBrowseBtn.AutoSize = true;

            Button exportDeckProfileBtn = new Button();
            exportDeckProfileBtn.Text = "Export Deck Profile";
            exportDeckProfileBtn.AutoSize = true;

            Button exportDeckProfileBrowseBtn = new Button();
            exportDeckProfileBrowseBtn.Text = "Browse";
            exportDeckProfileBrowseBtn.AutoSize = true;

            TableLayoutPanel profileButtonTableLayoutPanel = new TableLayoutPanel();
            profileButtonTableLayoutPanel.Width = 500;
            profileButtonTableLayoutPanel.Height = 50;
            profileButtonTableLayoutPanel.Anchor = AnchorStyles.Left;
            profileButtonTableLayoutPanel.Anchor = AnchorStyles.Top;
            profileButtonTableLayoutPanel.Margin = new Padding(0);
            profileButtonTableLayoutPanel.Padding = new Padding(0);

            Label deckProfileChangeWarningLabel = new Label();
            deckProfileChangeWarningLabel.Text = "This will change all settings!!";
            deckProfileChangeWarningLabel.Font = new Font("Comic Sans MS", 12);
            deckProfileChangeWarningLabel.AutoSize = true;

            TableLayoutPanel deckProfileTableLayoutPanel = new TableLayoutPanel();
            deckProfileTableLayoutPanel.Width = 500;
            deckProfileTableLayoutPanel.Height = 800;
            deckProfileTableLayoutPanel.Anchor = AnchorStyles.Left;
            deckProfileTableLayoutPanel.Anchor = AnchorStyles.Top;
            deckProfileTableLayoutPanel.Margin = new Padding(0);
            deckProfileTableLayoutPanel.Padding = new Padding(0);

            profileButtonTableLayoutPanel.Controls.Add(importDeckProfileBtn, 0, 0);
            profileButtonTableLayoutPanel.Controls.Add(exportDeckProfileBtn, 1, 0);

            deckProfileTableLayoutPanel.Controls.Add(DeckProfileSettingsTitleLabel, 0, 0);
            deckProfileTableLayoutPanel.Controls.Add(DeckProfileSettingsHullTitleLabel, 0, 1);
            deckProfileTableLayoutPanel.Controls.Add(DeckProfileSettingsHullTextBox, 0, 2);
            deckProfileTableLayoutPanel.Controls.Add(DeckProfileSettingsFilePathLabel, 0, 3);
            deckProfileTableLayoutPanel.Controls.Add(DeckProfileSettingsFilePathTextBox, 0, 4);
            deckProfileTableLayoutPanel.Controls.Add(profileButtonTableLayoutPanel, 0, 5);
            deckProfileTableLayoutPanel.Controls.Add(deckProfileChangeWarningLabel, 0, 6);

            formInputWindowTableLayoutPanel.Controls.Add(deckProfileTableLayoutPanel, 0, 0);
            #endregion
            //Event Handlers------------------------------------------------------------------------------------>>>
            #region Event Handlers
            importDeckProfileBtn.Click += delegate {
                DialogResult exportFirstSettingsDialogueResult = MessageBox.Show(new Form { TopMost = true },"This will completely overwrite your settings file.  Click 'Yes' to export settings file first.  Click 'No' to skip export and import a new file.  Press 'Cancel' to Cancel.  Your day is going to go well.", "Be very careful with this Shiz!", MessageBoxButtons.YesNoCancel);
                if (exportFirstSettingsDialogueResult == DialogResult.Yes || exportFirstSettingsDialogueResult == DialogResult.No) {
                    //Import is definitely happening now so save a backup copy of settings in case of error
                    File.Copy(".\\SSAManager.conf", ".\\temp.conf", true);
                    //If 'Yes' Export Old Settings then Import New Settings
                    if (exportFirstSettingsDialogueResult == DialogResult.Yes) {
                        if (exportSettingsFile()) {
                            ImportSettingsFile();
                        }
                    } else {
                        //Just Import
                        ImportSettingsFile();
                    }
                    //delete temporary backup of settings now that it is no longer needed
                    if (File.Exists(".\\temp.conf")) {
                        File.Delete(".\\temp.conf");
                    }

                } else { /*Do Nothin Ya'll, Yes or No wasn't pressed*/ }
            };
            exportDeckProfileBtn.Click += delegate {
                exportSettingsFile();
            };

            #endregion
        }
        #endregion
        //--------------------------------Main Window Event Handlers---------------------------------------------------------------------------------------->>>>
        private void MainMenuTreeView_AfterSelect(object sender, TreeViewEventArgs e) {
            SQTSupportFunctions.SupportFunctions.CleanConfig(configurationFilePath);
            // Get the selected node.
            TreeNode node = MainMenuTreeView.SelectedNode;

            // Render Console Output.
            Console.WriteLine(string.Format("You selected: {0}", node.Text));

            //Clear Form items and repopulate according to TreeView Node Selection
            formInputWindowTableLayoutPanel.Controls.Clear();

            if (node.Text.Equals("SSA Deck Manager 1.0")) {
                formInputWindowTableLayoutPanel.ColumnCount = 3;
                BuildWarningScreenInterface();
                return;
            }
            if (node.Text.Equals("Tests")) {
                formInputWindowTableLayoutPanel.ColumnCount = 3;
                BuildTestsInterface();
                return;
            }
            if (node.Text.Equals("Virtual Machine Functions")) {
                formInputWindowTableLayoutPanel.ColumnCount = 3;
                BuildVirtualMachineFunctionsInterface();
                return;
            }
            if (node.Text.Equals("Configuration Management")) {
                formInputWindowTableLayoutPanel.ColumnCount = 3;
                BuildConfigurationManagementInterface();
                return;
            }
            if (node.Text.Equals("Deck Settings")) {
                formInputWindowTableLayoutPanel.ColumnCount = 3;
                BuildDeckSettingsInterface();
                return;
            }
            if (node.Text.Equals("Internal Website Tests")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildInternalWebsiteTestsInterface();
                return;
            }
            if (node.Text.Equals("External Website Tests")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildExternalSiteTestInterface();
                return;
            }
            if (node.Text.Equals("Snapshot All Virtual Machines")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildSnapshotAllVMsInterface();
                return;
            }
            if (node.Text.Equals("Check VM Tools")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildCheckVMToolsInterface();
                return;
            }
            if (node.Text.Equals("Power On Machines")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildPowerOnMachinesInterface();
                return;
            }
            if (node.Text.Equals("Power Off Machines")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildPowerOffMachinesInterface();
                return;
            }
            if (node.Text.Equals("Export Deck")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildExportDeckInterface();
                return;
            }
            if (node.Text.Equals("Domain Ping Test")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildDomainPingTestInterface();
                return;
            }
            if (node.Text.Equals("Check RBAC")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildCheckRBACInterface();
                return;
            }
            if (node.Text.Equals("Check Java Versioning")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildCheckJavaVersioningInterface();
                return;
            }
            if (node.Text.Equals("Check All Services")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildCheckAllServicesInterface();
                return;
            }
            if (node.Text.Equals("Get Registry Data")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildGetRegistryDataInterface();
                return;
            }
            if (node.Text.Equals("Server Address Settings")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildServerSettingsInterface();
                return;
            }
            if (node.Text.Equals("Ship and User Settings")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildShipAndUserSettingsInterface();
                return;
            }
            if (node.Text.Equals("Windows & Local Settings")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildWindowsAndLocalSettingsInterface();
                return;
            }
            if (node.Text.Equals("Website List Settings")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildWebsitesSettingsInterface();
                return;
            }
            if (node.Text.Equals("Deck Profile Settings")) {
                formInputWindowTableLayoutPanel.ColumnCount = 1;
                BuildDeckProfileSettingsInterface();
                return;
            }
        }
        private void SQTTestInterface_Closed(object sender, FormClosedEventArgs e) {
            SQTSupportFunctions.SupportFunctions.CleanConfig(configurationFilePath);
            Environment.Exit(0);
        }

        //-------------------------------Local Support Functions----------------------------------------------------------------------------------------//
        private void RefreshUserListHulls(Hashtable windowsUserTextBoxHT, TextBox shipHullNumberTextBox) {
            foreach (DictionaryEntry pair in windowsUserTextBoxHT) {
                StringBuilder newUserValue = new StringBuilder();
                hullNumber = shipHullNumberTextBox.Text.Trim();
                TextBox tbToChange = (TextBox)windowsUserTextBoxHT[pair.Key];
                newUserValue.Append(tbToChange.Text.ToString().Trim());
                tbToChange.Text = hullNumber + "\\" + SQTSupportFunctions.SupportFunctions.RemoveHullFromUser(newUserValue.ToString());
            }
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
                if (line.StartsWith("[HULL]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    hullNumber = line.Replace("[HULL]", "");
                    domain = (hullNumber + ".navy.mil");
                    continue;
                }
                if (line.StartsWith("[ADIP]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    line = line.Replace("[ADIP]", "");
                    line.TrimEnd();
                    adIpAddressList = new ArrayList(line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                    continue;
                }
                if (line.StartsWith("[ESXIP]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    line = line.Replace("[ESXIP]", "");
                    line.TrimEnd();
                    esxIpList = new ArrayList(line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                    continue;
                }
                if (line.StartsWith("[REDHAT]")) {
                    Console.WriteLine("Getting info from config file... " + line);
                    line = line.Replace("[REDHAT]", "");
                    line.TrimEnd();
                    rhServerList = new ArrayList(line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                    continue;
                }
                if (line.StartsWith("[USER]")) {
                    if (line.StartsWith("[USER][REG]")) {
                        Console.WriteLine("Getting info from config file... " + line);
                        regUserName = line.Replace("[USER][REG]", hullNumber + "\\");
                        continue;
                    }
                    if (line.StartsWith("[USER][DA]")) {
                        Console.WriteLine("Getting info from config file... " + line);
                        domainAdminName = line.Replace("[USER][DA]", hullNumber + "\\");
                        continue;
                    }
                    if (line.StartsWith("[USER][SA]")) {
                        Console.WriteLine("Getting info from config file... " + line);
                        serverAdminName = line.Replace("[USER][SA]", hullNumber + "\\");
                        continue;
                    }
                    if (line.StartsWith("[USER][WA]")) {
                        Console.WriteLine("Getting info from config file... " + line);
                        workstationAdminName = line.Replace("[USER][WA]", hullNumber + "\\");
                        continue;
                    }
                    if (line.StartsWith("[USER][SSH]")) {
                        Console.WriteLine("Getting info from config file... " + line);
                        sshUserName = line.Replace("[USER][SSH]", "");
                        continue;
                    }
                    if (line.StartsWith("[USER][SM]")) {
                        Console.WriteLine("Getting info from config file... " + line);
                        smUserName = line.Replace("[USER][SM]", "");
                    }
                    if (line.StartsWith("[USER][ESX]")) {
                        Console.WriteLine("Getting info from config file... " + line);
                        esxUserName = line.Replace("[USER][ESX]", "");
                        continue;
                    }
                    if (line.StartsWith("[USER][LOC]")) {
                        Console.WriteLine("Getting info from config file... " + line);
                        localUserName = line.Replace("[USER][LOC]", "");
                        continue;
                    }
                }
                if (line.StartsWith("[WEBTEST]")) {
                    if (line.StartsWith("[WEBTEST][EXTERNAL]")) {
                        line = line.Replace("[WEBTEST][EXTERNAL]", "");
                        externalSiteList = new ArrayList(line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        continue;
                    }
                    if (line.StartsWith("[WEBTEST][INTERNAL]")) {
                        line = line.Replace("[WEBTEST][INTERNAL]", "");
                        internalSiteList = new ArrayList(line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        continue;
                    }
                }
            }
            configFileReader.Close();
        }
        private void ChangeDefaultTestPasswordSQTTestForm(object sender, EventArgs e) {
            defaultTestPwd = Interaction.InputBox("Please enter the default test password for the accounts in your domain: ", defaultPassMessageWindowTitle);
            defaultTestPwd = defaultTestPwd.Trim();
        }
        private bool ValidateUserAccountInput(Hashtable allTextBoxHT) {
            ArrayList errorList = new ArrayList();
            ArrayList adIPsCorrectFormat = new ArrayList();
            bool userInputIsValid = true;
            foreach (DictionaryEntry pair in allTextBoxHT) {
                TextBox currentTB = (TextBox)allTextBoxHT[pair.Key];
                switch (pair.Key) {
                    case "shipHullNumberTextBox":
                        if (!(SQTSupportFunctions.SupportFunctions.TestHullRegex(currentTB.Text.Trim()))) {
                            errorList.Add("Ship Hull: You can only use numbers and letters for the hull!");
                            userInputIsValid = false;
                        }
                        break;
                    case "shipADIpAddressTextBox":
                        string line = currentTB.Text.Trim();
                        ArrayList adIpAddressListToCheck = new ArrayList(line.Split(';'));
                        foreach (string ip in adIpAddressListToCheck) {
                            if (!ip.Equals("")) {
                                if (!(SupportFunctions.ValidateIp(ip))) {
                                    errorList.Add("Ship AD: Value entered (" + ip + ") is not an IP!");
                                    userInputIsValid = false;
                                } else {
                                    adIPsCorrectFormat.Add(ip);
                                }
                            }
                        }
                        StringBuilder newIPListSB = new StringBuilder();
                        currentTB.Text = "";
                        if (adIPsCorrectFormat.Count > 0) {
                            foreach (string ip in adIPsCorrectFormat) {
                                newIPListSB.Append(ip + ";");
                            }
                            currentTB.Text = newIPListSB.ToString();
                        } else {
                            currentTB.Text = "No Valid IPs Given!";
                        }
                        break;
                    case "regularUserNameTextBox":
                        if (!(SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                            errorList.Add("Regula rUser Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            userInputIsValid = false;
                        }
                        break;
                    case "domainAdminNameTextBox":
                        if (!(SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                            errorList.Add("Domain Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            userInputIsValid = false;
                        }
                        break;
                    case "serverAdminNameTextBox":
                        if (!(SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                            errorList.Add("Server Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            userInputIsValid = false;
                        }
                        break;
                    case "workstationAdminNameTextBox":
                        if (!(SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                            errorList.Add("Workstation Admin Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-One Backslash");
                            userInputIsValid = false;
                        }
                        break;
                    case "linuxSSHuserNameTextBox":
                        if (!(SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                            errorList.Add("Linux SSH User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods");
                            userInputIsValid = false;
                        }
                        break;
                    case "linuxSMUserNameTextBox":
                        if (!(SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                            errorList.Add("Linux SM User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods");
                            userInputIsValid = false;
                        }
                        break;
                    case "esxUserNameTextBox":
                        if (!(SupportFunctions.TestESXUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                            errorList.Add("ESX User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods-@");
                            userInputIsValid = false;
                        }
                        break;
                    case "locUserNameTextBox":
                        if (!(SupportFunctions.TestUserNameAndDomainRegex(currentTB.Text.Trim()))) {
                            errorList.Add("Local User Name Allowed Characters: Letters-Numbers-Hyphens-Underscores-Periods");
                            userInputIsValid = false;
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
                MessageBox.Show(new Form { TopMost = true },"I need you to fix these issues:\n" + errorListSB.ToString(), hullandUserValidatorMessageWindowTitle);

            }
            return userInputIsValid;
        }
        private static void TestLogonToRedHatMachine(SshClient client, ArrayList failedLoginList) {
            using (client) {
                client.ErrorOccurred += delegate {
                    //DO NOTHING 
                };
                try {
                    client.Connect();
                    SshCommand sc = client.CreateCommand("echo 'was granted access'");
                    sc.Execute();
                    string answer = sc.Result;
                    client.Disconnect();
                    if (answer.Contains("was granted access")) {
                        bool isValid = true;
                        Console.WriteLine("User: \'" + client.ConnectionInfo.Username + "\' on: \'" + client.ConnectionInfo.Host + "\' isValid: " + isValid);
                    }
                } catch (Exception e) {
                    switch (e.GetType().ToString()) {
                        case "System.Net.Sockets.SocketException":
                            //wrong IP
                            Console.WriteLine("IP Address: \'" + client.ConnectionInfo.Host + "\' is not a RedHat machine or is powered off!");
                            if (!failedLoginList.Contains("IP Address: \'" + client.ConnectionInfo.Host + "\' is not a RedHat Machine or is powered off!")) { failedLoginList.Add("IP Address: \'" + client.ConnectionInfo.Host + "\' is not a RedHat Machine or is powered off!"); }
                            break;
                        default:
                            //wrong username/password
                            Console.WriteLine("User: \'" + client.ConnectionInfo.Username + "\' on: \'" + client.ConnectionInfo.Host + "\' isValid: False ");
                            failedLoginList.Add("User: \'" + client.ConnectionInfo.Username + "\' on: \'" + client.ConnectionInfo.Host + "\'");
                            break;
                    }
                }
            }
        }
        private bool TestLogonToESXHost(ArrayList failedLoginList, string testUser) {
            return esxAgent.TestLogonToESXHost(failedLoginList, testUser,defaultTestPwd);
        }
        private void SaveWebsitesLists(ListView externalSitesToTestListView, ListView internalSitesToTestListView) {
            //variables
            ArrayList externalWebsiteToSaveList = new ArrayList(externalSitesToTestListView.Items);
            ArrayList internalWebsiteToSaveList = new ArrayList(internalSitesToTestListView.Items);
            StringBuilder externalSitesToSaveStringBuilder = new StringBuilder();
            StringBuilder internalSitesToSaveStringBuilder = new StringBuilder();

            //Create lines to save
            externalSitesToSaveStringBuilder.Append("[WEBTEST][EXTERNAL]");
            internalSitesToSaveStringBuilder.Append("[WEBTEST][INTERNAL]");
            foreach (ListViewItem item in externalWebsiteToSaveList) { externalSitesToSaveStringBuilder.Append(item.Text.Trim() + ";"); }
            foreach (ListViewItem item in internalWebsiteToSaveList) { internalSitesToSaveStringBuilder.Append(item.Text.Trim() + ";"); }

            //Read file and get specific line to replace
            StreamReader configFileReader = new StreamReader(configurationFilePath);
            string externalLineToReplace = "";
            string internalLineToReplace = "";
            configurationFilePath = Path.GetFullPath(configurationFilePath);
            Console.WriteLine("\nConfig File Location: " + configurationFilePath);
            string line;

            while ((line = configFileReader.ReadLine()) != null) {
                if (line.StartsWith("[WEBTEST][EXTERNAL]")) {
                    Console.WriteLine("Getting info... " + line);
                    externalLineToReplace = line.Trim();
                }
                if (line.StartsWith("[WEBTEST][INTERNAL]")) {
                    Console.WriteLine("Getting info... " + line);
                    internalLineToReplace = line.Trim();
                }
            }
            configFileReader.Close();

            //replace or add external sites
            String configFileText = File.ReadAllText(configurationFilePath);
            if (!externalLineToReplace.Equals("")) {
                //Replace old config line and set text file contents  
                configFileText = configFileText.Replace(externalLineToReplace, externalSitesToSaveStringBuilder.ToString());
            } else {
                //Append missing line
                StringBuilder addLineSB = new StringBuilder(File.ReadAllText(configurationFilePath));
                addLineSB.Append(externalSitesToSaveStringBuilder.ToString());
                configFileText = addLineSB.ToString();
            }
            //replace or add internal sites
            if (!internalLineToReplace.Equals("")) {
                //Replace old config line and set text file contents  
                configFileText = configFileText.Replace(internalLineToReplace, internalSitesToSaveStringBuilder.ToString());
            } else {
                //Append missing line
                StringBuilder addLineSB = new StringBuilder(File.ReadAllText(configurationFilePath));
                addLineSB.Append(internalSitesToSaveStringBuilder.ToString());
                configFileText = addLineSB.ToString();
            }
            try {
                File.WriteAllText(configurationFilePath, configFileText);
            } catch (Exception e) {
                MessageBox.Show(new Form { TopMost = true },"There was an error trying to write to your config file!\nPlease check the file isn't being used by another program and retry save action.\n" + e.Message, ipAddressValidatorMessageWindowTitle);
            }

            //Reload New Config Values
            CheckSettingsFiles();
            externalSitesToTestListView.Items.Clear();
            internalSitesToTestListView.Items.Clear();
            foreach (String s in externalSiteList) {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = s.Trim();
                externalSitesToTestListView.Items.Add(lvi);
            }
            foreach (String s in internalSiteList) {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = s.Trim();
                internalSitesToTestListView.Items.Add(lvi);
            }
        }
        private void CheckImportSettingsFilePrefixes(string importFilePath) {
            StreamReader configFileReader = new StreamReader(importFilePath);
            importFilePath = Path.GetFullPath(importFilePath);
            Console.WriteLine("\nConfig File Location: " + importFilePath);
            int lineCounter = 0;
            ArrayList foundList = new ArrayList();
            ArrayList notFoundList = new ArrayList();
            StringBuilder notFoundMessageAndList = new StringBuilder();
            string line;
            while ((line = configFileReader.ReadLine()) != null) {
                if (line.Length > (line.LastIndexOf(']') + 1)) {
                    line = line.Remove((line.LastIndexOf(']') + 1));
                }
                Console.WriteLine("\nLine: " + line);
                if (configSettingsLineList.Contains(line.Trim()) && !foundList.Contains(line.Trim())) {
                    lineCounter++;
                    foundList.Add(line.Trim());
                }

            }
            if (lineCounter < 15) {
                foreach (string prefix in configSettingsLineList) {
                    Console.WriteLine("\nChecking for prefix: " + prefix);
                    if (!foundList.Contains(prefix)) {
                        notFoundList.Add(prefix);
                        Console.WriteLine("\nPrefix Not Present: " + prefix);
                    }
                }
            }


            Console.WriteLine("\nFinal Line Counter: " + lineCounter);
            configFileReader.Close();

        }
        private void ImportSettingsFile() {
            //Choose Import File
            OpenFileDialog chooseImportFileDialog = new OpenFileDialog() {
                FileName = "",
                Filter = "Config Files (*.conf)|*.conf",
                Title = "Import-o-Matic - Select a \".conf\" File to load"
            };
            DialogResult chooseImportFileDialogueResult = chooseImportFileDialog.ShowDialog();
            //If file chosen and ok pressed, clean and import file
            if (chooseImportFileDialogueResult == DialogResult.OK) {
                MessageBox.Show(new Form { TopMost = true },"Importing File: " + chooseImportFileDialog.FileName, "Import-o-matic", MessageBoxButtons.OK);
                CheckImportSettingsFilePrefixes(chooseImportFileDialog.FileName);
                SQTSupportFunctions.SupportFunctions.CleanConfig(chooseImportFileDialog.FileName);
                if (!chooseImportFileDialog.FileName.ToUpper().Equals(Path.GetFullPath(".\\SSAManager.conf").ToUpper())) {
                    File.Copy(chooseImportFileDialog.FileName, ".\\SSAManager.conf", true);
                }
                formInputWindowTableLayoutPanel.Controls.Clear();
                CheckSettingsFiles();
                BuildDeckProfileSettingsInterface();
                DialogResult validateImportSettingsDialogueResult = MessageBox.Show(new Form { TopMost = true },"Deck Settings Imported Successfully! Would you like to validate the new settings? This will validate your settings screen by screen", "Validate-o-matic", MessageBoxButtons.YesNo);
                if (validateImportSettingsDialogueResult == DialogResult.Yes) {
                    //Run VALIDATION Checks from button clicks and validation methods!
                    //ESX IP SETTIINGS CHECK
                    formInputWindowTableLayoutPanel.Controls.Clear();
                    CheckSettingsFiles();
                    ArrayList serverIPValidationButtons = BuildServerSettingsInterface();//Interface returns validation buttons, click both of them
                    foreach (Button validationButton in serverIPValidationButtons) {
                        validationButton.PerformClick();
                    }
                    //Website format validation, remove sites that aren't valid
                    formInputWindowTableLayoutPanel.Controls.Clear();
                    BuildWebsitesSettingsInterface();
                    ArrayList badSites = new ArrayList();
                    //test format of each external site, add to bad list if not formatted propery
                    foreach (String site in externalSiteList) {
                        if (!SupportFunctions.TestWebsiteString(site, false)) {
                            badSites.Add(site);
                        };
                    }
                    //remove bad sites from list
                    foreach (String site in badSites) { externalSiteList.Remove(site); }
                    badSites.Clear();
                    //test format of each internal site, add to bad list if not formatted propery
                    foreach (String site in internalSiteList) {
                        if (!SupportFunctions.TestWebsiteString(site, false)) {
                            badSites.Add(site);
                        };
                    }
                    //remove bad sites from list
                    foreach (String site in badSites) { internalSiteList.Remove(site); }
                    badSites.Clear();
                    //Validate User and Hull Settings
                    formInputWindowTableLayoutPanel.Controls.Clear();
                    Button validateUserSettingsBtn = BuildShipAndUserSettingsInterface();//Interface returns validation button
                    validateUserSettingsBtn.PerformClick();
                    MessageBox.Show(new Form { TopMost = true },"Validation checks completed!  Have a day that is great!", "Validate-o-matic", MessageBoxButtons.OK);
                }
            } else {
                //stop all other actions and return
                return;
            }
        }
        private bool exportSettingsFile() {
            SaveFileDialog chooseExportFileDialog = new SaveFileDialog() {
                FileName = "Settings-Export-" + DateTime.Now.ToFileTime(),
                Filter = "Config Files (*.conf)|*.conf",
                Title = "Export-o-Matic - Select Where To Save Settings File"
            };
            DialogResult chooseExportFileDialogueResult = chooseExportFileDialog.ShowDialog();
            if (chooseExportFileDialogueResult == DialogResult.OK) {
                MessageBox.Show(new Form { TopMost = true },"Exporting to: " + chooseExportFileDialog.FileName, "Export-o-matic", MessageBoxButtons.OK);
                if (File.Exists(chooseExportFileDialog.FileName)) { File.Delete(chooseExportFileDialog.FileName); }
                File.Copy(".\\SSAManager.conf", chooseExportFileDialog.FileName);
                //Action complete
                return true;
            } else {
                //Action canceled
                return false;
            }
        }
        private void AddVMsToFullESXList() {
            if (esxAgent == null) { esxAgent = new EsxAgent(esxUserName, esxIpList); }
            fullVMListFromESX.Clear();
            foreach (VirtualMachine vm in esxAgent.GetListOfESXVMs(defaultTestPwd)) {
                fullVMListFromESX.Add(vm);
            }
        }
        private bool ConnectESX() {
            //validate ESX IP List
            bool allIsAreValid = true;
            if (esxIpList.Count > 0) {
                ArrayList tempList = new ArrayList();
                foreach (string ip in esxIpList) {
                    if (SupportFunctions.ValidateIp(ip)) {
                        tempList.Add(ip);                        
                    } else {
                        allIsAreValid = false;
                    }
                }
                esxIpList.Clear();
                esxIpList = tempList;
                if (!allIsAreValid){
                    if (esxIpList.Count > 0) {
                        MessageBox.Show(new Form { TopMost = true },"Some of your ESX Host Ips are invalid! I will attempt to connect to the valid addresses.  Invalid addresses will not appear in the interface but will remain in the SSAManager.conf configuration file until you go to Deck Settings>Server Address Settings and click [Save ESX IP Settings].", ipAddressValidatorMessageWindowTitle);
                    } else {
                        MessageBox.Show(new Form { TopMost = true },"All of your ESX Host Ips are invalid or missing in the configuration file, check ESX Host Settings!  No ESX connections will be made until you have at least one valid address", ipAddressValidatorMessageWindowTitle);
                        return false;
                    }
                }
                if (!esxUserName.Equals(noConfigString) && !esxUserName.Equals(null) && !esxUserName.Equals("<USERNAME>")) {
                    if (esxAgent == null) { esxAgent = new EsxAgent(esxUserName, esxIpList); }
                    if (esxAgent.MakeConnections(defaultTestPwd)) {
                        Console.WriteLine("ESX Connections Made!");
                    } else {
                        Console.WriteLine("Some or All ESX Connection Failed!");
                    }
                } else {
                    MessageBox.Show(new Form { TopMost = true },"Your ESX Host username is invalid or missing in the configuration file, check ESX Host Settings!", ipAddressValidatorMessageWindowTitle);
                    return false;
                }

            } else {
                MessageBox.Show(new Form { TopMost = true },"Your ESX Host is empty in the configuration file, check ESX Host Settings!", ipAddressValidatorMessageWindowTitle);
                return false;
            }
            Console.WriteLine("ESX Connections Complete!");
            return true;
        }

    }

}
