using System.Drawing;

namespace SQTTestInterface {
    partial class ComponentsLoadingWindow {
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
        private void InitializeComponent(string loadingMessage, Bitmap loadingAnimationName) {
            this.label1 = new System.Windows.Forms.Label();
            this.loadingGif = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadingGif)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gill Sans Ultra Bold Condensed", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = loadingMessage;
            // 
            // CyclopsGif
            // 
            this.loadingGif.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.loadingGif.Image = loadingAnimationName;
            this.loadingGif.Location = new System.Drawing.Point(12, 55);
            this.loadingGif.MaximumSize = new System.Drawing.Size(320, 315);
            this.loadingGif.MinimumSize = new System.Drawing.Size(320, 315);
            this.loadingGif.Name = "LoadingGif";
            this.loadingGif.Size = new System.Drawing.Size(320, 315);
            this.loadingGif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadingGif.TabIndex = 1;
            this.loadingGif.TabStop = false;
            // 
            // ComponentsLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 382);
            this.ControlBox = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowIcon = false;
            this.Controls.Add(this.loadingGif);
            this.Controls.Add(this.label1);
            this.Name = loadingMessage;
            ((System.ComponentModel.ISupportInitialize)(this.loadingGif)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox loadingGif;
    }
}