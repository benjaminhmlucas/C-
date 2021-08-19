using System.Collections;
using System.Windows.Forms;

namespace SQTTestInterface
{
    partial class MainInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainInterface));
            this.MenuBackgroundFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.MainMenuTreeView = new System.Windows.Forms.TreeView();
            this.formInputWindowTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainOuterTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.changeTestPasswordBtn = new System.Windows.Forms.Button();
            this.MenuBackgroundFlowLayoutPanel.SuspendLayout();
            this.mainOuterTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuBackgroundFlowLayoutPanel
            // 
            this.MenuBackgroundFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuBackgroundFlowLayoutPanel.Controls.Add(this.changeTestPasswordBtn);
            this.MenuBackgroundFlowLayoutPanel.Controls.Add(this.MainMenuTreeView);
            this.MenuBackgroundFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MenuBackgroundFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MenuBackgroundFlowLayoutPanel.Name = "MenuBackgroundFlowLayoutPanel";
            this.MenuBackgroundFlowLayoutPanel.Size = new System.Drawing.Size(193, 458);
            this.MenuBackgroundFlowLayoutPanel.TabIndex = 0;
            // 
            // MainMenuTreeView
            // 
            this.MainMenuTreeView.Location = new System.Drawing.Point(0, 30);
            this.MainMenuTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.MainMenuTreeView.Name = "MainMenuTreeView";
            this.MainMenuTreeView.Size = new System.Drawing.Size(190, 418);
            this.MainMenuTreeView.TabIndex = 0;
            this.MainMenuTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MainMenuTreeView_AfterSelect);
            // 
            // formInputWindowTableLayoutPanel
            // 
            this.formInputWindowTableLayoutPanel.AutoScroll = true;
            this.formInputWindowTableLayoutPanel.AutoSize = true;
            this.formInputWindowTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.formInputWindowTableLayoutPanel.ColumnCount = 1;
            this.formInputWindowTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.formInputWindowTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 587F));
            this.formInputWindowTableLayoutPanel.Location = new System.Drawing.Point(193, 0);
            this.formInputWindowTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.formInputWindowTableLayoutPanel.MinimumSize = new System.Drawing.Size(500, 500);
            this.formInputWindowTableLayoutPanel.Name = "formInputWindowTableLayoutPanel";
            this.formInputWindowTableLayoutPanel.RowCount = 1;
            this.formInputWindowTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.formInputWindowTableLayoutPanel.Size = new System.Drawing.Size(500, 500);
            this.formInputWindowTableLayoutPanel.TabIndex = 1;
            // 
            // mainOuterTableLayoutPanel
            // 
            this.mainOuterTableLayoutPanel.AutoScroll = true;
            this.mainOuterTableLayoutPanel.AutoSize = true;
            this.mainOuterTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainOuterTableLayoutPanel.ColumnCount = 2;
            this.mainOuterTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.5F));
            this.mainOuterTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.5F));
            this.mainOuterTableLayoutPanel.Controls.Add(this.formInputWindowTableLayoutPanel, 1, 0);
            this.mainOuterTableLayoutPanel.Controls.Add(this.MenuBackgroundFlowLayoutPanel, 0, 0);
            this.mainOuterTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainOuterTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainOuterTableLayoutPanel.Name = "mainOuterTableLayoutPanel";
            this.mainOuterTableLayoutPanel.RowCount = 1;
            this.mainOuterTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.52941F));
            this.mainOuterTableLayoutPanel.Size = new System.Drawing.Size(757, 500);
            this.mainOuterTableLayoutPanel.TabIndex = 0;
            // 
            // changeTestPasswordBtn
            // 
            this.changeTestPasswordBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeTestPasswordBtn.Location = new System.Drawing.Point(0, 0);
            this.changeTestPasswordBtn.Margin = new System.Windows.Forms.Padding(0);
            this.changeTestPasswordBtn.Name = "changeTestPasswordBtn";
            this.changeTestPasswordBtn.Size = new System.Drawing.Size(192, 30);
            this.changeTestPasswordBtn.TabIndex = 1;
            this.changeTestPasswordBtn.Text = "Change Default Test Password";
            this.changeTestPasswordBtn.UseVisualStyleBackColor = true;
            this.changeTestPasswordBtn.Click += new System.EventHandler(this.ChangeDefaultTestPasswordSQTTestForm);
            // 
            // MainInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.mainOuterTableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MainInterface";
            this.Text = "SQT Test Interface";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SQTTestInterface_Closed);
            this.MenuBackgroundFlowLayoutPanel.ResumeLayout(false);
            this.mainOuterTableLayoutPanel.ResumeLayout(false);
            this.mainOuterTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel MenuBackgroundFlowLayoutPanel;
        private System.Windows.Forms.TreeView MainMenuTreeView;
        private System.Windows.Forms.TableLayoutPanel formInputWindowTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel mainOuterTableLayoutPanel;
        private Button changeTestPasswordBtn;
    }
}

