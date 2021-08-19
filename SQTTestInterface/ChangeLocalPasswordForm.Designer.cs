namespace SQTTestInterface {
    partial class ChangeLocalPasswordForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeLocalPasswordForm));
            this.titleLabelChangePasswordForm = new System.Windows.Forms.Label();
            this.oldPassLabel = new System.Windows.Forms.Label();
            this.newPass1Label = new System.Windows.Forms.Label();
            this.newPass2Label = new System.Windows.Forms.Label();
            this.oldPassTextBox = new System.Windows.Forms.TextBox();
            this.newPass1TextBox = new System.Windows.Forms.TextBox();
            this.newPass2TextBox = new System.Windows.Forms.TextBox();
            this.submitBtnChangePasswordForm = new System.Windows.Forms.Button();
            this.cancelBtnChangePasswordForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabelChangePasswordForm
            // 
            this.titleLabelChangePasswordForm.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.titleLabelChangePasswordForm.Location = new System.Drawing.Point(12, 9);
            this.titleLabelChangePasswordForm.Name = "titleLabelChangePasswordForm";
            this.titleLabelChangePasswordForm.Size = new System.Drawing.Size(285, 38);
            this.titleLabelChangePasswordForm.TabIndex = 0;
            this.titleLabelChangePasswordForm.Text = "Please enter your current password.  Then enter the new password twice.";
            this.titleLabelChangePasswordForm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // oldPassLabel
            // 
            this.oldPassLabel.AutoSize = true;
            this.oldPassLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.oldPassLabel.Location = new System.Drawing.Point(9, 57);
            this.oldPassLabel.Name = "oldPassLabel";
            this.oldPassLabel.Size = new System.Drawing.Size(75, 13);
            this.oldPassLabel.TabIndex = 1;
            this.oldPassLabel.Text = "Old Password:";
            // 
            // newPass1Label
            // 
            this.newPass1Label.AutoSize = true;
            this.newPass1Label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.newPass1Label.Location = new System.Drawing.Point(9, 82);
            this.newPass1Label.Name = "newPass1Label";
            this.newPass1Label.Size = new System.Drawing.Size(90, 13);
            this.newPass1Label.TabIndex = 2;
            this.newPass1Label.Text = "New Password 1:";
            // 
            // newPass2Label
            // 
            this.newPass2Label.AutoSize = true;
            this.newPass2Label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.newPass2Label.Location = new System.Drawing.Point(9, 108);
            this.newPass2Label.Name = "newPass2Label";
            this.newPass2Label.Size = new System.Drawing.Size(90, 13);
            this.newPass2Label.TabIndex = 3;
            this.newPass2Label.Text = "New Password 2:";
            // 
            // oldPassTextBox
            // 
            this.oldPassTextBox.Location = new System.Drawing.Point(105, 54);
            this.oldPassTextBox.Name = "oldPassTextBox";
            this.oldPassTextBox.Size = new System.Drawing.Size(192, 20);
            this.oldPassTextBox.TabIndex = 4;
            // 
            // newPass1TextBox
            // 
            this.newPass1TextBox.Location = new System.Drawing.Point(105, 79);
            this.newPass1TextBox.Name = "newPass1TextBox";
            this.newPass1TextBox.Size = new System.Drawing.Size(192, 20);
            this.newPass1TextBox.TabIndex = 5;
            // 
            // newPass2TextBox
            // 
            this.newPass2TextBox.Location = new System.Drawing.Point(105, 105);
            this.newPass2TextBox.Name = "newPass2TextBox";
            this.newPass2TextBox.Size = new System.Drawing.Size(192, 20);
            this.newPass2TextBox.TabIndex = 6;
            // 
            // submitBtnChangePasswordForm
            // 
            this.submitBtnChangePasswordForm.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.submitBtnChangePasswordForm.Location = new System.Drawing.Point(140, 141);
            this.submitBtnChangePasswordForm.Name = "submitBtnChangePasswordForm";
            this.submitBtnChangePasswordForm.Size = new System.Drawing.Size(75, 23);
            this.submitBtnChangePasswordForm.TabIndex = 7;
            this.submitBtnChangePasswordForm.Text = "Submit";
            this.submitBtnChangePasswordForm.UseVisualStyleBackColor = true;
            this.submitBtnChangePasswordForm.Click += new System.EventHandler(this.SubmitPasswordChange);
            // 
            // cancelBtnChangePasswordForm
            // 
            this.cancelBtnChangePasswordForm.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cancelBtnChangePasswordForm.Location = new System.Drawing.Point(221, 141);
            this.cancelBtnChangePasswordForm.Name = "cancelBtnChangePasswordForm";
            this.cancelBtnChangePasswordForm.Size = new System.Drawing.Size(75, 23);
            this.cancelBtnChangePasswordForm.TabIndex = 8;
            this.cancelBtnChangePasswordForm.Text = "Cancel";
            this.cancelBtnChangePasswordForm.UseVisualStyleBackColor = true;
            this.cancelBtnChangePasswordForm.Click += new System.EventHandler(this.CancelPasswordChange);
            // 
            // ChangeLocalPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 177);
            this.Controls.Add(this.cancelBtnChangePasswordForm);
            this.Controls.Add(this.submitBtnChangePasswordForm);
            this.Controls.Add(this.newPass2TextBox);
            this.Controls.Add(this.newPass1TextBox);
            this.Controls.Add(this.oldPassTextBox);
            this.Controls.Add(this.newPass2Label);
            this.Controls.Add(this.newPass1Label);
            this.Controls.Add(this.oldPassLabel);
            this.Controls.Add(this.titleLabelChangePasswordForm);
            this.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChangeLocalPasswordForm";
            this.Text = "Change Local Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label oldPassLabel;
        private System.Windows.Forms.Label newPass1Label;
        private System.Windows.Forms.Label newPass2Label;
        private System.Windows.Forms.TextBox oldPassTextBox;
        private System.Windows.Forms.TextBox newPass1TextBox;
        private System.Windows.Forms.TextBox newPass2TextBox;
        private System.Windows.Forms.Button submitBtnChangePasswordForm;
        private System.Windows.Forms.Button cancelBtnChangePasswordForm;
        private System.Windows.Forms.Label titleLabelChangePasswordForm;
    }
}