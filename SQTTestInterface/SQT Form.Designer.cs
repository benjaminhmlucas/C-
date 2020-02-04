using System.Collections;

namespace SQTTestInterface
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.MenuBackgroundFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.MainMenuTreeView = new System.Windows.Forms.TreeView();
            this.FormInputWindowTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainOuterTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MenuBackgroundFlowLayoutPanel.SuspendLayout();
            this.mainOuterTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuBackgroundFlowLayoutPanel
            // 
            this.MenuBackgroundFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuBackgroundFlowLayoutPanel.Controls.Add(this.MainMenuTreeView);
            this.MenuBackgroundFlowLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.MenuBackgroundFlowLayoutPanel.Name = "MenuBackgroundFlowLayoutPanel";
            this.MenuBackgroundFlowLayoutPanel.Size = new System.Drawing.Size(193, 435);
            this.MenuBackgroundFlowLayoutPanel.TabIndex = 0;
            // 
            // MainMenuTreeView
            // 
            this.MainMenuTreeView.Location = new System.Drawing.Point(3, 3);
            this.MainMenuTreeView.Name = "MainMenuTreeView";
            this.MainMenuTreeView.Size = new System.Drawing.Size(190, 431);
            this.MainMenuTreeView.TabIndex = 0;
            this.MainMenuTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MainMenuTreeView_AfterSelect);
            // 
            // FormInputWindowTableLayoutPanel
            // 
            this.FormInputWindowTableLayoutPanel.AutoScroll = true;
            this.FormInputWindowTableLayoutPanel.AutoSize = true;
            this.FormInputWindowTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FormInputWindowTableLayoutPanel.ColumnCount = 2;
            this.FormInputWindowTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FormInputWindowTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 587F));
            this.FormInputWindowTableLayoutPanel.Location = new System.Drawing.Point(205, 3);
            this.FormInputWindowTableLayoutPanel.MinimumSize = new System.Drawing.Size(500, 435);
            this.FormInputWindowTableLayoutPanel.Name = "FormInputWindowTableLayoutPanel";
            this.FormInputWindowTableLayoutPanel.RowCount = 1;
            this.FormInputWindowTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.FormInputWindowTableLayoutPanel.Size = new System.Drawing.Size(587, 435);
            this.FormInputWindowTableLayoutPanel.TabIndex = 1;
            // 
            // mainOuterTableLayoutPanel
            // 
            this.mainOuterTableLayoutPanel.AutoScroll = true;
            this.mainOuterTableLayoutPanel.AutoSize = true;
            this.mainOuterTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainOuterTableLayoutPanel.ColumnCount = 2;
            this.mainOuterTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.5F));
            this.mainOuterTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.5F));
            this.mainOuterTableLayoutPanel.Controls.Add(this.FormInputWindowTableLayoutPanel, 1, 0);
            this.mainOuterTableLayoutPanel.Controls.Add(this.MenuBackgroundFlowLayoutPanel, 0, 0);
            this.mainOuterTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainOuterTableLayoutPanel.Name = "mainOuterTableLayoutPanel";
            this.mainOuterTableLayoutPanel.RowCount = 1;
            this.mainOuterTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.52941F));
            this.mainOuterTableLayoutPanel.Size = new System.Drawing.Size(796, 441);
            this.mainOuterTableLayoutPanel.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainOuterTableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "SQT Test Interface";
            this.MenuBackgroundFlowLayoutPanel.ResumeLayout(false);
            this.mainOuterTableLayoutPanel.ResumeLayout(false);
            this.mainOuterTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel MenuBackgroundFlowLayoutPanel;
        private System.Windows.Forms.TreeView MainMenuTreeView;
        private System.Windows.Forms.TableLayoutPanel FormInputWindowTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel mainOuterTableLayoutPanel;
    }
}

