namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
	partial class AssociationDialog
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
      this.txtStartRole = new System.Windows.Forms.TextBox();
      this.cboEndMultiplicity = new System.Windows.Forms.ComboBox();
      this.txtEndRole = new System.Windows.Forms.TextBox();
      this.cboStartMultiplicity = new System.Windows.Forms.ComboBox();
      this.picArrow = new System.Windows.Forms.PictureBox();
      this.txtName = new System.Windows.Forms.TextBox();
      this.txtSecond = new System.Windows.Forms.TextBox();
      this.txtFirst = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.picArrow)).BeginInit();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(408, 182);
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
      this.btnOK.Location = new System.Drawing.Point(286, 182);
      this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(112, 35);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
      // 
      // txtStartRole
      // 
      this.txtStartRole.Location = new System.Drawing.Point(18, 135);
      this.txtStartRole.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.txtStartRole.Name = "txtStartRole";
      this.txtStartRole.Size = new System.Drawing.Size(186, 26);
      this.txtStartRole.TabIndex = 2;
      // 
      // cboEndMultiplicity
      // 
      this.cboEndMultiplicity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cboEndMultiplicity.FormattingEnabled = true;
      this.cboEndMultiplicity.Items.AddRange(new object[] {
            "1",
            "0..1",
            "0..*",
            "1..*",
            "*"});
      this.cboEndMultiplicity.Location = new System.Drawing.Point(446, 61);
      this.cboEndMultiplicity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cboEndMultiplicity.Name = "cboEndMultiplicity";
      this.cboEndMultiplicity.Size = new System.Drawing.Size(73, 28);
      this.cboEndMultiplicity.TabIndex = 1;
      // 
      // txtEndRole
      // 
      this.txtEndRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtEndRole.Location = new System.Drawing.Point(333, 135);
      this.txtEndRole.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.txtEndRole.Name = "txtEndRole";
      this.txtEndRole.Size = new System.Drawing.Size(186, 26);
      this.txtEndRole.TabIndex = 3;
      this.txtEndRole.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // cboStartMultiplicity
      // 
      this.cboStartMultiplicity.FormattingEnabled = true;
      this.cboStartMultiplicity.Items.AddRange(new object[] {
            "1",
            "0..1",
            "0..*",
            "1..*",
            "*"});
      this.cboStartMultiplicity.Location = new System.Drawing.Point(18, 61);
      this.cboStartMultiplicity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cboStartMultiplicity.Name = "cboStartMultiplicity";
      this.cboStartMultiplicity.Size = new System.Drawing.Size(73, 28);
      this.cboStartMultiplicity.TabIndex = 6;
      // 
      // picArrow
      // 
      this.picArrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.picArrow.Location = new System.Drawing.Point(18, 103);
      this.picArrow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.picArrow.Name = "picArrow";
      this.picArrow.Size = new System.Drawing.Size(502, 23);
      this.picArrow.TabIndex = 4;
      this.picArrow.TabStop = false;
      this.picArrow.Paint += new System.Windows.Forms.PaintEventHandler(this.PicArrow_Paint);
      this.picArrow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicArrow_MouseDown);
      // 
      // txtName
      // 
      this.txtName.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.txtName.Location = new System.Drawing.Point(148, 61);
      this.txtName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.txtName.Name = "txtName";
      this.txtName.Size = new System.Drawing.Size(238, 26);
      this.txtName.TabIndex = 0;
      this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // txtSecond
      // 
      this.txtSecond.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSecond.Location = new System.Drawing.Point(388, 16);
      this.txtSecond.Name = "txtSecond";
      this.txtSecond.ReadOnly = true;
      this.txtSecond.Size = new System.Drawing.Size(132, 26);
      this.txtSecond.TabIndex = 10;
      this.txtSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // txtFirst
      // 
      this.txtFirst.Location = new System.Drawing.Point(19, 17);
      this.txtFirst.Name = "txtFirst";
      this.txtFirst.ReadOnly = true;
      this.txtFirst.Size = new System.Drawing.Size(132, 26);
      this.txtFirst.TabIndex = 9;
      // 
      // AssociationDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(538, 235);
      this.Controls.Add(this.txtSecond);
      this.Controls.Add(this.txtFirst);
      this.Controls.Add(this.txtName);
      this.Controls.Add(this.cboStartMultiplicity);
      this.Controls.Add(this.txtEndRole);
      this.Controls.Add(this.picArrow);
      this.Controls.Add(this.cboEndMultiplicity);
      this.Controls.Add(this.txtStartRole);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AssociationDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Edit Association";
      ((System.ComponentModel.ISupportInitialize)(this.picArrow)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox txtStartRole;
		private System.Windows.Forms.ComboBox cboEndMultiplicity;
		private System.Windows.Forms.PictureBox picArrow;
		private System.Windows.Forms.TextBox txtEndRole;
		private System.Windows.Forms.ComboBox cboStartMultiplicity;
		private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.TextBox txtSecond;
    private System.Windows.Forms.TextBox txtFirst;
  }
}