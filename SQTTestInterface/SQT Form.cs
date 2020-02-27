using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SQTTestInterface {
    public partial class Form1 : Form {

        private int ExternalSiteTestLines = 1;
        private string logFilePath = @".\\ExternalSiteTestLog.txt";
        private List<ComboBox> ComputerControlComboBoxList = new List<ComboBox>(5);
        private List<ListView> SitesControlListViewList = new List<ListView>(5);
        public Form1() {
            InitializeComponent();
            BuildTreeViewMenu();
        }
        //Create Form Data------------------------------------------------------------------------->>>>
        private void BuildTreeViewMenu() {
            ArrayList menuTopList = new ArrayList(new string[] { "SQT Tests", "Virtual Machine Functions", "Configuration Management", "SQT Deck Information" });
            ArrayList menuSQTTestItems = new ArrayList(new string[] { "External Website Tests", "Internal Website Tests", "All Sites Test" });
            ArrayList menuVMFunctionsItems = new ArrayList(new string[] { "Snapshot All", "Delete Snapshot", "Revert Deck" });
            ArrayList menuConfigManagementItems = new ArrayList(new string[] { "Backup Registy", "Restore Registry", "Check GPOs" });
            ArrayList menuSQTInformationItems = new ArrayList(new string[] { "ESX Host List", "VM List", "Domain Settings" });
            ArrayList listToUse = new ArrayList();
            foreach(String item in menuTopList) {
                this.MainMenuTreeView.Nodes.Add(item);
            }
            foreach(TreeNode node in MainMenuTreeView.Nodes) {
                if(node.Text.Equals("SQT Tests")) {
                    listToUse = menuSQTTestItems;
                }
                if(node.Text.Equals("Virtual Machine Functions")) {
                    listToUse = menuVMFunctionsItems;
                }
                if(node.Text.Equals("Configuration Management")) {
                    listToUse = menuConfigManagementItems;
                }
                if(node.Text.Equals("SQT Deck Information")) {
                    listToUse = menuSQTInformationItems;
                }
                foreach(String item in listToUse) {
                    node.Nodes.Add(item);
                }
            }
        }

        private void BuildExternalSiteTestInterface() {
            FormInputWindowTableLayoutPanel.ColumnCount = 1;
            int i;

            ArrayList computerListToTestFromList = new ArrayList(new string[] { "Choose Workstation To Test From","KMS1", "KMS2", "KMS3", "KMS4", "KMS5","All Worstations" });
            ArrayList externalSiteList = new ArrayList(new string[] { "http://www.public.navy.mil/bupers-npc/", "https://cmpropac.nmci.navy.mil/", "https://fedvte.usalearning.gov", "https://sailor.navy.mil/" });
            ArrayList browserList = new ArrayList(new string[] { "Internet Explorer", "Firefox", "Chrome"});

            for (i = 0;i < ExternalSiteTestLines;i++) {
                BuildSingleExternalSiteTestLine(i, computerListToTestFromList, externalSiteList, browserList);
            }

            //create External test controls

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

            FormInputWindowTableLayoutPanel.Controls.Add(BottomDivider, 0, (i + 1));
            FormInputWindowTableLayoutPanel.Controls.Add(AddRemoveTestLinesTableLayoutPanel, 0, (i + 2));

            AddTestLineBtn.Click += delegate{
                if (ExternalSiteTestLines < 5)
                {
                    ExternalSiteTestLines++;
                    FormInputWindowTableLayoutPanel.Controls.Clear();
                    BuildExternalSiteTestInterface();
                } else {
                    MessageBox.Show("You cannot run than five tests!", "Action Canceled!");
                }
            };

            RemoveTestLineBtn.Click += delegate {
                if (ExternalSiteTestLines > 1)
                {
                    ExternalSiteTestLines--;
                    SitesControlListViewList.RemoveAt(SitesControlListViewList.Count-1);
                    FormInputWindowTableLayoutPanel.Controls.Clear();
                    BuildExternalSiteTestInterface();
                } else {
                    MessageBox.Show("You cannot run less than one test!","Action Canceled!");
                }
            };

            RunTestsBtn.Click += delegate {
                int testCounter = 1;
                List<string> checkedSites = new List<string>();
                StringBuilder sb = new StringBuilder();
                //Create/clear Log File
                
                File.Create(logFilePath).Close();
                File.WriteAllText(logFilePath, "");
                
                foreach (ListView lv in SitesControlListViewList) {
                    ComboBox cb = ComputerControlComboBoxList[testCounter - 1];
                    if (cb.Text.Equals("Choose Workstation To Test From")) {
                        MessageBox.Show("Error! You must choose a computer to test from for test: " + testCounter);
                        return;
                    }
                    if (lv.CheckedItems.Count == 0) {
                        MessageBox.Show("Error! You must choose at least one site to connect to for test: " + testCounter);
                        return;
                    }
                    sb.Clear();
                    foreach (ListViewItem lvi in lv.Items) {
                        if (lvi.Checked == true) {
                            Console.WriteLine("Running Test: " + testCounter + " From Computer: "+ cb.Text + " Connecting To: " + lvi.Text);
                            sb.Append("\""+lvi.Text+"\",");
                        }
                    }
                    sb.Remove(sb.Length-1,1);
                    Console.WriteLine(sb);
                    testCounter++;
                    if (cb.Text.Equals("All Worstations")) {
                        cb.Text = "KMS";
                    }
                    try {
                        //Start PowerShell Script for external testing
                        startProcess("powershell.exe", (".\\SQTExternalSiteTest.ps1 -TestFromComputer " + cb.Text + " -SiteToConnectTo " + sb),true);
                    } catch (Exception e) {
                        MessageBox.Show("There was an error running the PowerShell script: "+e.Message);
                    }
                }
                startProcess("notepad.exe",logFilePath,true);
            };
        }

        private void BuildSingleExternalSiteTestLine(int lineCounter, ArrayList ComputerToTestFromList, ArrayList ExternalSiteList, ArrayList browserList) {

            TableLayoutPanel SingleTestLinePanel = new TableLayoutPanel();
            SingleTestLinePanel.Width = 600;
            SingleTestLinePanel.Height = 280;
            SingleTestLinePanel.VerticalScroll.Enabled = true;

            Label TopDivider = new Label();
            TopDivider.Text = ("Start of TEST NO.{"+ (lineCounter + 1) + "}-------------------------------------------------------------------------------------------------------------->>");
            TopDivider.Width = 600;
            TopDivider.Height = 15;

            Label ComputerToTestFromLabel = new Label();
            ComputerToTestFromLabel.Text = "Computer/Worstation to Connect From:";
            ComputerToTestFromLabel.Width = 600;
            ComputerToTestFromLabel.Height = 15;

            ComboBox ComputerToTestFromComboBox;
            try {
                ComputerToTestFromComboBox = ComputerControlComboBoxList[lineCounter];
            } catch (Exception e) {
                //MessageBox.Show("Error:" + e.Message); 
                ComputerToTestFromComboBox = new ComboBox();
                foreach (String s in ComputerToTestFromList) {
                    ComputerToTestFromComboBox.Items.Add(s);
                }
                ComputerToTestFromComboBox.Width = 240;
                ComputerToTestFromComboBox.SelectedIndex = 0;
                ComputerToTestFromComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                ComputerControlComboBoxList.Add(ComputerToTestFromComboBox);
            }

            Label browsersToUse = new Label();
            browsersToUse.Text = "\nBrowser(s) To User:";
            browsersToUse.Height = 15;
            ListView browsersListView = new ListView();
            foreach (String browserName in browserList) {
                browsersListView.Items.Add(browserName);
            }
            browsersListView.Height = 63;
            browsersListView.Width = 140;            
            browsersListView.CheckBoxes = true;
            browsersListView.View = View.List;
            browsersListView.BorderStyle = BorderStyle.None;
            browsersListView.BackColor = Control.DefaultBackColor;
            
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

            ListView SitesToTestList;
            try {
                SitesToTestList = SitesControlListViewList[lineCounter];
            } catch (Exception e) {
                //MessageBox.Show("Error:" + e.Message); 
                SitesToTestList = new ListView();
                SitesToTestList.CheckBoxes = true;
                foreach (String s in ExternalSiteList) {
                    SitesToTestList.Items.Add(s);
                }
                SitesToTestList.View = View.List;
                SitesToTestList.BorderStyle = BorderStyle.None;
                SitesToTestList.BackColor = Control.DefaultBackColor;
                SitesToTestList.Width = 240;
                SitesToTestList.Height = 77;
                SitesControlListViewList.Add(SitesToTestList);
            }



            SingleTestLinePanel.Controls.Add(TopDivider, 0, 0);
            SingleTestLinePanel.Controls.Add(ComputerToTestFromLabel, 0, 1);
            SingleTestLinePanel.Controls.Add(ComputerToTestFromComboBox, 0, 2);
            SingleTestLinePanel.Controls.Add(browsersToUse, 0, 3);
            SingleTestLinePanel.Controls.Add(browsersListView, 0, 4);
            SingleTestLinePanel.Controls.Add(SitesToTestLabel, 0, 5);
            SingleTestLinePanel.Controls.Add(SelectButtonsTableLayoutPanel, 0, 6);
            SingleTestLinePanel.Controls.Add(SitesToTestList, 0, 7);
            FormInputWindowTableLayoutPanel.Controls.Add(SingleTestLinePanel, 0, lineCounter);

            SelectAllBtn.Click += delegate {
                foreach(ListViewItem item in SitesToTestList.Items) {
                    item.Checked = true;
                }
            };
            DeselectAllBtn.Click += delegate {
                foreach(ListViewItem item in SitesToTestList.Items) {
                    item.Checked = false;
                }
            };
        }

        public void startProcess(string processName,string arguments,Boolean runSynchronously) {
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

        //Event Handlers------------------------------------------------------------------------>>>>
        private void MainMenuTreeView_AfterSelect(object sender, TreeViewEventArgs e) {
            // Get the selected node.
            TreeNode node = MainMenuTreeView.SelectedNode;
            // Render message box.
            //MessageBox.Show(string.Format("You selected: {0}", node.Text));
            // Render Console Output.
            Console.WriteLine(string.Format("You selected: {0}", node.Text));
            
            //Clear Form items and repopulate according to TreeView Node Selection
            FormInputWindowTableLayoutPanel.Controls.Clear();

            if (node.Text.Equals("External Website Tests")) {
                BuildExternalSiteTestInterface();
            }
        }
    }
}
