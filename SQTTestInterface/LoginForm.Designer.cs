using System;

namespace SQTTestInterface {
    partial class LoginForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.loginConnectBtn = new System.Windows.Forms.Button();
            this.loginCancelBtn = new System.Windows.Forms.Button();
            this.loginHeaderLabel = new System.Windows.Forms.Label();
            this.loginDirectionsLabel = new System.Windows.Forms.Label();
            this.adIPTextBox = new System.Windows.Forms.TextBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.adIPLabel = new System.Windows.Forms.Label();
            this.usernNameLabel = new System.Windows.Forms.Label();
            this.pwdLoginLabel = new System.Windows.Forms.Label();
            this.pwdLoginTextBox = new System.Windows.Forms.TextBox();
            this.connectLocalCheckBox = new System.Windows.Forms.CheckBox();
            this.mainLoginPanel = new System.Windows.Forms.Panel();
            this.defaultConfigBtn = new System.Windows.Forms.Button();
            this.mainLoginPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // loginConnectBtn
            // 
            this.loginConnectBtn.Location = new System.Drawing.Point(223, 217);
            this.loginConnectBtn.Name = "loginConnectBtn";
            this.loginConnectBtn.Size = new System.Drawing.Size(75, 23);
            this.loginConnectBtn.TabIndex = 0;
            this.loginConnectBtn.Text = "Connect";
            this.loginConnectBtn.UseVisualStyleBackColor = true;
            this.loginConnectBtn.Click += new System.EventHandler(this.LoginConnect_Clicked);
            // 
            // loginCancelBtn
            // 
            this.loginCancelBtn.Location = new System.Drawing.Point(304, 217);
            this.loginCancelBtn.Name = "loginCancelBtn";
            this.loginCancelBtn.Size = new System.Drawing.Size(75, 23);
            this.loginCancelBtn.TabIndex = 1;
            this.loginCancelBtn.Text = "Cancel";
            this.loginCancelBtn.UseVisualStyleBackColor = true;
            this.loginCancelBtn.Click += new System.EventHandler(this.LoginCancel_Clicked);
            // 
            // loginHeaderLabel
            // 
            this.loginHeaderLabel.AutoSize = true;
            this.loginHeaderLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.loginHeaderLabel.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginHeaderLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.loginHeaderLabel.Location = new System.Drawing.Point(43, 14);
            this.loginHeaderLabel.Name = "loginHeaderLabel";
            this.loginHeaderLabel.Size = new System.Drawing.Size(311, 31);
            this.loginHeaderLabel.TabIndex = 0;
            this.loginHeaderLabel.Text = "Welcome To Domain Commander!";
            this.loginHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loginDirectionsLabel
            // 
            this.loginDirectionsLabel.AutoSize = true;
            this.loginDirectionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginDirectionsLabel.Location = new System.Drawing.Point(26, 45);
            this.loginDirectionsLabel.Name = "loginDirectionsLabel";
            this.loginDirectionsLabel.Size = new System.Drawing.Size(349, 20);
            this.loginDirectionsLabel.TabIndex = 3;
            this.loginDirectionsLabel.Text = "Please enter the connection info to your domain:";
            // 
            // adIPTextBox
            // 
            this.adIPTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.adIPTextBox.Location = new System.Drawing.Point(110, 130);
            this.adIPTextBox.Name = "adIPTextBox";
            this.adIPTextBox.Size = new System.Drawing.Size(267, 20);
            this.adIPTextBox.TabIndex = 4;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(110, 162);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(267, 20);
            this.userNameTextBox.TabIndex = 6;
            // 
            // adIPLabel
            // 
            this.adIPLabel.AutoSize = true;
            this.adIPLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.adIPLabel.Location = new System.Drawing.Point(6, 133);
            this.adIPLabel.Name = "adIPLabel";
            this.adIPLabel.Size = new System.Drawing.Size(98, 13);
            this.adIPLabel.TabIndex = 5;
            this.adIPLabel.Text = "Active Directory IP:";
            // 
            // usernNameLabel
            // 
            this.usernNameLabel.AutoSize = true;
            this.usernNameLabel.Location = new System.Drawing.Point(6, 165);
            this.usernNameLabel.Name = "usernNameLabel";
            this.usernNameLabel.Size = new System.Drawing.Size(104, 13);
            this.usernNameLabel.TabIndex = 7;
            this.usernNameLabel.Text = "User Name(No Hull):";
            // 
            // pwdLoginLabel
            // 
            this.pwdLoginLabel.AutoSize = true;
            this.pwdLoginLabel.Location = new System.Drawing.Point(6, 194);
            this.pwdLoginLabel.Name = "pwdLoginLabel";
            this.pwdLoginLabel.Size = new System.Drawing.Size(56, 13);
            this.pwdLoginLabel.TabIndex = 9;
            this.pwdLoginLabel.Text = "Password:";
            // 
            // pwdLoginTextBox
            // 
            this.pwdLoginTextBox.Location = new System.Drawing.Point(110, 191);
            this.pwdLoginTextBox.Name = "pwdLoginTextBox";
            this.pwdLoginTextBox.PasswordChar = '*';
            this.pwdLoginTextBox.Size = new System.Drawing.Size(267, 20);
            this.pwdLoginTextBox.TabIndex = 8;
            // 
            // connectLocalCheckBox
            // 
            this.connectLocalCheckBox.AutoSize = true;
            this.connectLocalCheckBox.Location = new System.Drawing.Point(11, 83);
            this.connectLocalCheckBox.Name = "connectLocalCheckBox";
            this.connectLocalCheckBox.Size = new System.Drawing.Size(155, 17);
            this.connectLocalCheckBox.TabIndex = 10;
            this.connectLocalCheckBox.Text = "Connect with local account";
            this.connectLocalCheckBox.UseVisualStyleBackColor = true;
            this.connectLocalCheckBox.CheckedChanged += new System.EventHandler(this.ConnectLocalCheckBox_CheckedChanged);
            // 
            // mainLoginPanel
            // 
            this.mainLoginPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainLoginPanel.Controls.Add(this.defaultConfigBtn);
            this.mainLoginPanel.Controls.Add(this.connectLocalCheckBox);
            this.mainLoginPanel.Controls.Add(this.pwdLoginLabel);
            this.mainLoginPanel.Controls.Add(this.pwdLoginTextBox);
            this.mainLoginPanel.Controls.Add(this.usernNameLabel);
            this.mainLoginPanel.Controls.Add(this.userNameTextBox);
            this.mainLoginPanel.Controls.Add(this.adIPLabel);
            this.mainLoginPanel.Controls.Add(this.adIPTextBox);
            this.mainLoginPanel.Controls.Add(this.loginDirectionsLabel);
            this.mainLoginPanel.Controls.Add(this.loginHeaderLabel);
            this.mainLoginPanel.Controls.Add(this.loginCancelBtn);
            this.mainLoginPanel.Controls.Add(this.loginConnectBtn);
            this.mainLoginPanel.Location = new System.Drawing.Point(12, 12);
            this.mainLoginPanel.Name = "mainLoginPanel";
            this.mainLoginPanel.Size = new System.Drawing.Size(389, 254);
            this.mainLoginPanel.TabIndex = 11;
            // 
            // defaultConfigBtn
            // 
            this.defaultConfigBtn.Location = new System.Drawing.Point(251, 79);
            this.defaultConfigBtn.Name = "defaultConfigBtn";
            this.defaultConfigBtn.Size = new System.Drawing.Size(128, 23);
            this.defaultConfigBtn.TabIndex = 11;
            this.defaultConfigBtn.Text = "Reset to Default Config";
            this.defaultConfigBtn.UseVisualStyleBackColor = true;
            this.defaultConfigBtn.Click += new System.EventHandler(this.DefaultConfigBtn_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 279);
            this.Controls.Add(this.mainLoginPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.Text = "Domain Login";
            this.TransparencyKey = System.Drawing.Color.LightPink;
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.mainLoginPanel.ResumeLayout(false);
            this.mainLoginPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loginConnectBtn;
        private System.Windows.Forms.Button loginCancelBtn;
        private System.Windows.Forms.Label loginHeaderLabel;
        private System.Windows.Forms.Label loginDirectionsLabel;
        private System.Windows.Forms.TextBox adIPTextBox;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Label adIPLabel;
        private System.Windows.Forms.Label usernNameLabel;
        private System.Windows.Forms.Label pwdLoginLabel;
        private System.Windows.Forms.TextBox pwdLoginTextBox;
        private System.Windows.Forms.CheckBox connectLocalCheckBox;
        private System.Windows.Forms.Panel mainLoginPanel;
        private System.Windows.Forms.Button defaultConfigBtn;
    }
}