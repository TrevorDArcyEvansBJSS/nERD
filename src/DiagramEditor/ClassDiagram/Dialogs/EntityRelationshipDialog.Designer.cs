namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
	partial class EntityRelationshipDialog
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
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.cboEndMultiplicity = new System.Windows.Forms.ComboBox();
      this.cboStartMultiplicity = new System.Windows.Forms.ComboBox();
      this.picArrow = new System.Windows.Forms.PictureBox();
      this.txtName = new System.Windows.Forms.TextBox();
      this.txtFirst = new System.Windows.Forms.TextBox();
      this.txtSecond = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.picArrow)).BeginInit();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(408, 145);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(112, 35);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Location = new System.Drawing.Point(286, 145);
      this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(112, 35);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
      // 
      // cboEndMultiplicity
      // 
      this.cboEndMultiplicity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cboEndMultiplicity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboEndMultiplicity.FormattingEnabled = true;
      this.cboEndMultiplicity.Items.AddRange(new object[] {
            "0..1",
            "1",
            "0..*",
            "1..*"});
      this.cboEndMultiplicity.Location = new System.Drawing.Point(446, 58);
      this.cboEndMultiplicity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cboEndMultiplicity.Name = "cboEndMultiplicity";
      this.cboEndMultiplicity.Size = new System.Drawing.Size(73, 28);
      this.cboEndMultiplicity.TabIndex = 1;
      this.cboEndMultiplicity.SelectedIndexChanged += new System.EventHandler(this.CboEndMultiplicity_SelectedIndexChanged);
      // 
      // cboStartMultiplicity
      // 
      this.cboStartMultiplicity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboStartMultiplicity.FormattingEnabled = true;
      this.cboStartMultiplicity.Items.AddRange(new object[] {
            "0..1",
            "1",
            "0..*",
            "1..*"});
      this.cboStartMultiplicity.Location = new System.Drawing.Point(18, 58);
      this.cboStartMultiplicity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cboStartMultiplicity.Name = "cboStartMultiplicity";
      this.cboStartMultiplicity.Size = new System.Drawing.Size(73, 28);
      this.cboStartMultiplicity.TabIndex = 6;
      this.cboStartMultiplicity.SelectedIndexChanged += new System.EventHandler(this.CboStartMultiplicity_SelectedIndexChanged);
      // 
      // picArrow
      // 
      this.picArrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.picArrow.Location = new System.Drawing.Point(18, 100);
      this.picArrow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.picArrow.Name = "picArrow";
      this.picArrow.Size = new System.Drawing.Size(502, 23);
      this.picArrow.TabIndex = 4;
      this.picArrow.TabStop = false;
      this.picArrow.Paint += new System.Windows.Forms.PaintEventHandler(this.PicArrow_Paint);
      // 
      // txtName
      // 
      this.txtName.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.txtName.Location = new System.Drawing.Point(148, 58);
      this.txtName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.txtName.Name = "txtName";
      this.txtName.Size = new System.Drawing.Size(238, 26);
      this.txtName.TabIndex = 0;
      this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // txtFirst
      // 
      this.txtFirst.Location = new System.Drawing.Point(18, 13);
      this.txtFirst.Name = "txtFirst";
      this.txtFirst.ReadOnly = true;
      this.txtFirst.Size = new System.Drawing.Size(132, 26);
      this.txtFirst.TabIndex = 7;
      // 
      // txtSecond
      // 
      this.txtSecond.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSecond.Location = new System.Drawing.Point(387, 12);
      this.txtSecond.Name = "txtSecond";
      this.txtSecond.ReadOnly = true;
      this.txtSecond.Size = new System.Drawing.Size(132, 26);
      this.txtSecond.TabIndex = 8;
      this.txtSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // EntityRelationshipDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(538, 198);
      this.Controls.Add(this.txtSecond);
      this.Controls.Add(this.txtFirst);
      this.Controls.Add(this.txtName);
      this.Controls.Add(this.cboStartMultiplicity);
      this.Controls.Add(this.picArrow);
      this.Controls.Add(this.cboEndMultiplicity);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "EntityRelationshipDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Edit Entity Relationships";
      ((System.ComponentModel.ISupportInitialize)(this.picArrow)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ComboBox cboEndMultiplicity;
		private System.Windows.Forms.PictureBox picArrow;
		private System.Windows.Forms.ComboBox cboStartMultiplicity;
		private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.TextBox txtFirst;
    private System.Windows.Forms.TextBox txtSecond;
  }
}