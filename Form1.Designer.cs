namespace SalesApp {
    partial class Form1 {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.peopleComboBox = new System.Windows.Forms.ComboBox();
            this.salesPersonBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.RegionComboBox = new System.Windows.Forms.ComboBox();
            this.salesRegionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.refreshSalesButton = new System.Windows.Forms.Button();
            this.salesDataGridView = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.targetButton = new System.Windows.Forms.Button();
            this.newAmountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.newDateDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.newSalesPersonComboBox = new System.Windows.Forms.ComboBox();
            this.newRegionComboBox = new System.Windows.Forms.ComboBox();
            this.newSalesButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.salesPersonBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.salesRegionBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.salesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saleBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newAmountNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // peopleComboBox
            // 
            this.peopleComboBox.DataSource = this.salesPersonBindingSource;
            this.peopleComboBox.DisplayMember = "FullName";
            this.peopleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.peopleComboBox.FormattingEnabled = true;
            this.peopleComboBox.Location = new System.Drawing.Point(310, 57);
            this.peopleComboBox.Name = "peopleComboBox";
            this.peopleComboBox.Size = new System.Drawing.Size(174, 21);
            this.peopleComboBox.TabIndex = 0;
            this.peopleComboBox.ValueMember = "Id";
            // 
            // salesPersonBindingSource
            // 
            this.salesPersonBindingSource.DataSource = typeof(SalesApp.Models.SalesPerson);
            // 
            // RegionComboBox
            // 
            this.RegionComboBox.DataSource = this.salesRegionBindingSource;
            this.RegionComboBox.DisplayMember = "Name";
            this.RegionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RegionComboBox.FormattingEnabled = true;
            this.RegionComboBox.Location = new System.Drawing.Point(490, 57);
            this.RegionComboBox.Name = "RegionComboBox";
            this.RegionComboBox.Size = new System.Drawing.Size(174, 21);
            this.RegionComboBox.TabIndex = 1;
            this.RegionComboBox.ValueMember = "Id";
            // 
            // salesRegionBindingSource
            // 
            this.salesRegionBindingSource.DataSource = typeof(SalesApp.Models.SalesRegion);
            // 
            // refreshSalesButton
            // 
            this.refreshSalesButton.Location = new System.Drawing.Point(670, 55);
            this.refreshSalesButton.Name = "refreshSalesButton";
            this.refreshSalesButton.Size = new System.Drawing.Size(75, 23);
            this.refreshSalesButton.TabIndex = 2;
            this.refreshSalesButton.Text = "Refresh";
            this.refreshSalesButton.UseVisualStyleBackColor = true;
            this.refreshSalesButton.Click += new System.EventHandler(this.RefreshSalesButton_Click);
            // 
            // salesDataGridView
            // 
            this.salesDataGridView.AllowUserToAddRows = false;
            this.salesDataGridView.AutoGenerateColumns = false;
            this.salesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.salesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.amountDataGridViewTextBoxColumn,
            this.dateDataGridViewTextBoxColumn,
            this.UpdatedBy});
            this.salesDataGridView.DataSource = this.saleBindingSource;
            this.salesDataGridView.Location = new System.Drawing.Point(12, 120);
            this.salesDataGridView.Name = "salesDataGridView";
            this.salesDataGridView.Size = new System.Drawing.Size(738, 173);
            this.salesDataGridView.TabIndex = 3;
            this.salesDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.SalesDataGridView_CellEndEdit);
            this.salesDataGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.SalesDataGridView_UserDeletingRow);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            dataGridViewCellStyle1.Format = "C2";
            dataGridViewCellStyle1.NullValue = null;
            this.amountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            // 
            // dateDataGridViewTextBoxColumn
            // 
            this.dateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dateDataGridViewTextBoxColumn.DataPropertyName = "Date";
            dataGridViewCellStyle2.Format = "d";
            dataGridViewCellStyle2.NullValue = null;
            this.dateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.dateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
            this.dateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // UpdatedBy
            // 
            this.UpdatedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UpdatedBy.DataPropertyName = "UpdatedBy";
            this.UpdatedBy.HeaderText = "Updated By";
            this.UpdatedBy.Name = "UpdatedBy";
            this.UpdatedBy.ReadOnly = true;
            // 
            // saleBindingSource
            // 
            this.saleBindingSource.DataSource = typeof(SalesApp.Models.Sale);
            // 
            // targetButton
            // 
            this.targetButton.Location = new System.Drawing.Point(229, 55);
            this.targetButton.Name = "targetButton";
            this.targetButton.Size = new System.Drawing.Size(75, 23);
            this.targetButton.TabIndex = 4;
            this.targetButton.Text = "Target";
            this.targetButton.UseVisualStyleBackColor = true;
            this.targetButton.Click += new System.EventHandler(this.TargetButton_Click);
            // 
            // newAmountNumericUpDown
            // 
            this.newAmountNumericUpDown.DecimalPlaces = 2;
            this.newAmountNumericUpDown.Location = new System.Drawing.Point(12, 300);
            this.newAmountNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.newAmountNumericUpDown.Name = "newAmountNumericUpDown";
            this.newAmountNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.newAmountNumericUpDown.TabIndex = 5;
            // 
            // newDateDateTimePicker
            // 
            this.newDateDateTimePicker.Location = new System.Drawing.Point(12, 327);
            this.newDateDateTimePicker.Name = "newDateDateTimePicker";
            this.newDateDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.newDateDateTimePicker.TabIndex = 6;
            // 
            // newSalesPersonComboBox
            // 
            this.newSalesPersonComboBox.DataSource = this.salesPersonBindingSource;
            this.newSalesPersonComboBox.DisplayMember = "FullName";
            this.newSalesPersonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.newSalesPersonComboBox.FormattingEnabled = true;
            this.newSalesPersonComboBox.Location = new System.Drawing.Point(12, 353);
            this.newSalesPersonComboBox.Name = "newSalesPersonComboBox";
            this.newSalesPersonComboBox.Size = new System.Drawing.Size(174, 21);
            this.newSalesPersonComboBox.TabIndex = 7;
            this.newSalesPersonComboBox.ValueMember = "Id";
            // 
            // newRegionComboBox
            // 
            this.newRegionComboBox.DataSource = this.salesRegionBindingSource;
            this.newRegionComboBox.DisplayMember = "Name";
            this.newRegionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.newRegionComboBox.FormattingEnabled = true;
            this.newRegionComboBox.Location = new System.Drawing.Point(12, 380);
            this.newRegionComboBox.Name = "newRegionComboBox";
            this.newRegionComboBox.Size = new System.Drawing.Size(174, 21);
            this.newRegionComboBox.TabIndex = 8;
            this.newRegionComboBox.ValueMember = "Id";
            // 
            // newSalesButton
            // 
            this.newSalesButton.Location = new System.Drawing.Point(193, 380);
            this.newSalesButton.Name = "newSalesButton";
            this.newSalesButton.Size = new System.Drawing.Size(75, 23);
            this.newSalesButton.TabIndex = 9;
            this.newSalesButton.Text = "New Sale";
            this.newSalesButton.UseVisualStyleBackColor = true;
            this.newSalesButton.Click += new System.EventHandler(this.NewSalesButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 450);
            this.Controls.Add(this.newSalesButton);
            this.Controls.Add(this.newRegionComboBox);
            this.Controls.Add(this.newSalesPersonComboBox);
            this.Controls.Add(this.newDateDateTimePicker);
            this.Controls.Add(this.newAmountNumericUpDown);
            this.Controls.Add(this.targetButton);
            this.Controls.Add(this.salesDataGridView);
            this.Controls.Add(this.refreshSalesButton);
            this.Controls.Add(this.RegionComboBox);
            this.Controls.Add(this.peopleComboBox);
            this.Name = "Form1";
            this.Text = "a";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.salesPersonBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.salesRegionBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.salesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saleBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newAmountNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox peopleComboBox;
        private System.Windows.Forms.BindingSource salesPersonBindingSource;
        private System.Windows.Forms.ComboBox RegionComboBox;
        private System.Windows.Forms.BindingSource salesRegionBindingSource;
        private System.Windows.Forms.Button refreshSalesButton;
        private System.Windows.Forms.DataGridView salesDataGridView;
        private System.Windows.Forms.BindingSource saleBindingSource;
        private System.Windows.Forms.Button targetButton;
        private System.Windows.Forms.NumericUpDown newAmountNumericUpDown;
        private System.Windows.Forms.DateTimePicker newDateDateTimePicker;
        private System.Windows.Forms.ComboBox newSalesPersonComboBox;
        private System.Windows.Forms.ComboBox newRegionComboBox;
        private System.Windows.Forms.Button newSalesButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdatedBy;
    }
}

