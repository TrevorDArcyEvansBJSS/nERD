namespace NClass.CodeGenerator
{
	partial class Dialog
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
		private void InitializeComponent()
		{
      this.chkUseTabs = new System.Windows.Forms.CheckBox();
      this.btnGenerate = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.updIndentSize = new System.Windows.Forms.NumericUpDown();
      this.lblIndentSize = new System.Windows.Forms.Label();
      this.lstImportList = new System.Windows.Forms.ListBox();
      this.importToolStrip = new System.Windows.Forms.ToolStrip();
      this.toolDelete = new System.Windows.Forms.ToolStripButton();
      this.toolMoveDown = new System.Windows.Forms.ToolStripButton();
      this.toolMoveUp = new System.Windows.Forms.ToolStripButton();
      this.toolImportList = new System.Windows.Forms.ToolStripLabel();
      this.txtNewImport = new System.Windows.Forms.TextBox();
      this.btnAddItem = new System.Windows.Forms.Button();
      this.grpCodeStyle = new System.Windows.Forms.GroupBox();
      this.chkNotImplemented = new System.Windows.Forms.CheckBox();
      this.lblSolutionType = new System.Windows.Forms.Label();
      this.cboSolutionType = new System.Windows.Forms.ComboBox();
      this.cboLanguage = new System.Windows.Forms.ComboBox();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.btnBrowse = new System.Windows.Forms.Button();
      this.txtDestination = new System.Windows.Forms.TextBox();
      this.lblDestination = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.updIndentSize)).BeginInit();
      this.importToolStrip.SuspendLayout();
      this.grpCodeStyle.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // chkUseTabs
      // 
      this.chkUseTabs.AutoSize = true;
      this.chkUseTabs.Location = new System.Drawing.Point(20, 31);
      this.chkUseTabs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.chkUseTabs.Name = "chkUseTabs";
      this.chkUseTabs.Size = new System.Drawing.Size(178, 24);
      this.chkUseTabs.TabIndex = 0;
      this.chkUseTabs.Text = "Use tabs for indents";
      this.chkUseTabs.UseVisualStyleBackColor = true;
      this.chkUseTabs.CheckedChanged += new System.EventHandler(this.chkUseTabs_CheckedChanged);
      // 
      // btnGenerate
      // 
      this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnGenerate.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnGenerate.Location = new System.Drawing.Point(639, 374);
      this.btnGenerate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnGenerate.Name = "btnGenerate";
      this.btnGenerate.Size = new System.Drawing.Size(112, 35);
      this.btnGenerate.TabIndex = 10;
      this.btnGenerate.Text = "Generate";
      this.btnGenerate.UseVisualStyleBackColor = true;
      this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(760, 374);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(112, 35);
      this.btnCancel.TabIndex = 11;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // updIndentSize
      // 
      this.updIndentSize.Location = new System.Drawing.Point(20, 94);
      this.updIndentSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.updIndentSize.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
      this.updIndentSize.Name = "updIndentSize";
      this.updIndentSize.Size = new System.Drawing.Size(123, 26);
      this.updIndentSize.TabIndex = 2;
      this.updIndentSize.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
      // 
      // lblIndentSize
      // 
      this.lblIndentSize.AutoSize = true;
      this.lblIndentSize.Enabled = false;
      this.lblIndentSize.Location = new System.Drawing.Point(15, 69);
      this.lblIndentSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblIndentSize.Name = "lblIndentSize";
      this.lblIndentSize.Size = new System.Drawing.Size(91, 20);
      this.lblIndentSize.TabIndex = 1;
      this.lblIndentSize.Text = "Indent size:";
      // 
      // lstImportList
      // 
      this.lstImportList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstImportList.FormattingEnabled = true;
      this.lstImportList.ItemHeight = 20;
      this.lstImportList.Location = new System.Drawing.Point(18, 138);
      this.lstImportList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.lstImportList.Name = "lstImportList";
      this.lstImportList.Size = new System.Drawing.Size(426, 224);
      this.lstImportList.TabIndex = 4;
      this.lstImportList.SelectedValueChanged += new System.EventHandler(this.lstImportList_SelectedValueChanged);
      this.lstImportList.Leave += new System.EventHandler(this.lstImportList_Leave);
      // 
      // importToolStrip
      // 
      this.importToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.importToolStrip.AutoSize = false;
      this.importToolStrip.Dock = System.Windows.Forms.DockStyle.None;
      this.importToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.importToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.importToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolDelete,
            this.toolMoveDown,
            this.toolMoveUp,
            this.toolImportList});
      this.importToolStrip.Location = new System.Drawing.Point(112, 95);
      this.importToolStrip.Name = "importToolStrip";
      this.importToolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
      this.importToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this.importToolStrip.Size = new System.Drawing.Size(333, 38);
      this.importToolStrip.TabIndex = 3;
      this.importToolStrip.Text = "toolStrip1";
      // 
      // toolDelete
      // 
      this.toolDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolDelete.Enabled = false;
      this.toolDelete.Image = global::NClass.CodeGenerator.Properties.Resources.Delete;
      this.toolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolDelete.Name = "toolDelete";
      this.toolDelete.Size = new System.Drawing.Size(34, 33);
      this.toolDelete.Text = "Delete";
      this.toolDelete.Click += new System.EventHandler(this.toolDelete_Click);
      // 
      // toolMoveDown
      // 
      this.toolMoveDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolMoveDown.Enabled = false;
      this.toolMoveDown.Image = global::NClass.CodeGenerator.Properties.Resources.MoveDown;
      this.toolMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolMoveDown.Name = "toolMoveDown";
      this.toolMoveDown.Size = new System.Drawing.Size(34, 33);
      this.toolMoveDown.Text = "Move Down";
      this.toolMoveDown.Click += new System.EventHandler(this.toolMoveDown_Click);
      // 
      // toolMoveUp
      // 
      this.toolMoveUp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolMoveUp.Enabled = false;
      this.toolMoveUp.Image = global::NClass.CodeGenerator.Properties.Resources.MoveUp;
      this.toolMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolMoveUp.Name = "toolMoveUp";
      this.toolMoveUp.Size = new System.Drawing.Size(34, 33);
      this.toolMoveUp.Text = "Move Up";
      this.toolMoveUp.Click += new System.EventHandler(this.toolMoveUp_Click);
      // 
      // toolImportList
      // 
      this.toolImportList.Name = "toolImportList";
      this.toolImportList.Size = new System.Drawing.Size(94, 33);
      this.toolImportList.Text = "Import list";
      // 
      // txtNewImport
      // 
      this.txtNewImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtNewImport.Location = new System.Drawing.Point(18, 377);
      this.txtNewImport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.txtNewImport.Name = "txtNewImport";
      this.txtNewImport.Size = new System.Drawing.Size(304, 26);
      this.txtNewImport.TabIndex = 5;
      this.txtNewImport.TextChanged += new System.EventHandler(this.txtNewImport_TextChanged);
      this.txtNewImport.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNewImport_KeyDown);
      // 
      // btnAddItem
      // 
      this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddItem.Enabled = false;
      this.btnAddItem.Location = new System.Drawing.Point(333, 374);
      this.btnAddItem.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnAddItem.Name = "btnAddItem";
      this.btnAddItem.Size = new System.Drawing.Size(112, 35);
      this.btnAddItem.TabIndex = 6;
      this.btnAddItem.Text = "Add item";
      this.btnAddItem.UseVisualStyleBackColor = true;
      this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
      // 
      // grpCodeStyle
      // 
      this.grpCodeStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.grpCodeStyle.Controls.Add(this.chkNotImplemented);
      this.grpCodeStyle.Controls.Add(this.lblIndentSize);
      this.grpCodeStyle.Controls.Add(this.updIndentSize);
      this.grpCodeStyle.Controls.Add(this.chkUseTabs);
      this.grpCodeStyle.Location = new System.Drawing.Point(466, 177);
      this.grpCodeStyle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.grpCodeStyle.Name = "grpCodeStyle";
      this.grpCodeStyle.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.grpCodeStyle.Size = new System.Drawing.Size(406, 185);
      this.grpCodeStyle.TabIndex = 9;
      this.grpCodeStyle.TabStop = false;
      this.grpCodeStyle.Text = "Code style";
      // 
      // chkNotImplemented
      // 
      this.chkNotImplemented.AutoSize = true;
      this.chkNotImplemented.Location = new System.Drawing.Point(20, 143);
      this.chkNotImplemented.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.chkNotImplemented.Name = "chkNotImplemented";
      this.chkNotImplemented.Size = new System.Drawing.Size(362, 24);
      this.chkNotImplemented.TabIndex = 4;
      this.chkNotImplemented.Text = "Fill methods with \'Not implemented\' exceptions";
      this.chkNotImplemented.UseVisualStyleBackColor = true;
      // 
      // lblSolutionType
      // 
      this.lblSolutionType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblSolutionType.AutoSize = true;
      this.lblSolutionType.Location = new System.Drawing.Point(462, 103);
      this.lblSolutionType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lblSolutionType.Name = "lblSolutionType";
      this.lblSolutionType.Size = new System.Drawing.Size(148, 20);
      this.lblSolutionType.TabIndex = 12;
      this.lblSolutionType.Text = "Type of solution file:";
      // 
      // cboSolutionType
      // 
      this.cboSolutionType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cboSolutionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboSolutionType.FormattingEnabled = true;
      this.cboSolutionType.Items.AddRange(new object[] {
            "Visual Studio 2017",
            "Visual Studio 2019"});
      this.cboSolutionType.Location = new System.Drawing.Point(466, 128);
      this.cboSolutionType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cboSolutionType.Name = "cboSolutionType";
      this.cboSolutionType.Size = new System.Drawing.Size(404, 28);
      this.cboSolutionType.TabIndex = 13;
      // 
      // cboLanguage
      // 
      this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboLanguage.FormattingEnabled = true;
      this.cboLanguage.Items.AddRange(new object[] {
            "C#",
            "SQL"});
      this.cboLanguage.Location = new System.Drawing.Point(18, 98);
      this.cboLanguage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cboLanguage.Name = "cboLanguage";
      this.cboLanguage.Size = new System.Drawing.Size(88, 28);
      this.cboLanguage.TabIndex = 14;
      this.cboLanguage.SelectedIndexChanged += new System.EventHandler(this.cboLanguage_SelectedIndexChanged);
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.Controls.Add(this.btnBrowse, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.txtDestination, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.lblDestination, 0, 0);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 18);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(855, 57);
      this.tableLayoutPanel1.TabIndex = 15;
      // 
      // btnBrowse
      // 
      this.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.btnBrowse.Location = new System.Drawing.Point(743, 11);
      this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new System.Drawing.Size(112, 35);
      this.btnBrowse.TabIndex = 6;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.UseVisualStyleBackColor = true;
      this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
      // 
      // txtDestination
      // 
      this.txtDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDestination.Location = new System.Drawing.Point(98, 15);
      this.txtDestination.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.txtDestination.Name = "txtDestination";
      this.txtDestination.Size = new System.Drawing.Size(637, 26);
      this.txtDestination.TabIndex = 5;
      // 
      // lblDestination
      // 
      this.lblDestination.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.lblDestination.AutoSize = true;
      this.lblDestination.Location = new System.Drawing.Point(0, 17);
      this.lblDestination.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
      this.lblDestination.Name = "lblDestination";
      this.lblDestination.Size = new System.Drawing.Size(94, 20);
      this.lblDestination.TabIndex = 4;
      this.lblDestination.Text = "Destination:";
      // 
      // Dialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(891, 426);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.cboLanguage);
      this.Controls.Add(this.cboSolutionType);
      this.Controls.Add(this.lblSolutionType);
      this.Controls.Add(this.grpCodeStyle);
      this.Controls.Add(this.btnAddItem);
      this.Controls.Add(this.txtNewImport);
      this.Controls.Add(this.importToolStrip);
      this.Controls.Add(this.lstImportList);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnGenerate);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(889, 445);
      this.Name = "Dialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Code Generation";
      ((System.ComponentModel.ISupportInitialize)(this.updIndentSize)).EndInit();
      this.importToolStrip.ResumeLayout(false);
      this.importToolStrip.PerformLayout();
      this.grpCodeStyle.ResumeLayout(false);
      this.grpCodeStyle.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkUseTabs;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.NumericUpDown updIndentSize;
		private System.Windows.Forms.Label lblIndentSize;
		private System.Windows.Forms.ListBox lstImportList;
		private System.Windows.Forms.ToolStrip importToolStrip;
		private System.Windows.Forms.ToolStripButton toolDelete;
		private System.Windows.Forms.ToolStripButton toolMoveDown;
		private System.Windows.Forms.ToolStripButton toolMoveUp;
		private System.Windows.Forms.ToolStripLabel toolImportList;
		private System.Windows.Forms.TextBox txtNewImport;
		private System.Windows.Forms.Button btnAddItem;
		private System.Windows.Forms.GroupBox grpCodeStyle;
		private System.Windows.Forms.Label lblSolutionType;
		private System.Windows.Forms.ComboBox cboSolutionType;
		private System.Windows.Forms.CheckBox chkNotImplemented;
		private System.Windows.Forms.ComboBox cboLanguage;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtDestination;
		private System.Windows.Forms.Label lblDestination;
	}
}