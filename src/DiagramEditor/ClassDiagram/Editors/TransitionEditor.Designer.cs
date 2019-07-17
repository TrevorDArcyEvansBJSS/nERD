namespace NClass.DiagramEditor.ClassDiagram.Editors
{
  partial class TransitionEditor
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.txtName = new NClass.DiagramEditor.ClassDiagram.Editors.BorderedTextBox();
      // 
      // txtName
      // 
      this.txtName.AcceptsTab = true;
      this.txtName.Location = new System.Drawing.Point(4, 11);
      this.txtName.Name = "txtName";
      this.txtName.Padding = new System.Windows.Forms.Padding(1);
      this.txtName.ReadOnly = false;
      this.txtName.SelectionStart = 0;
      this.txtName.Size = new System.Drawing.Size(322, 26);
      this.txtName.TabIndex = 5;
      this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
      this.txtName.LostFocus += new System.EventHandler(this.txtName_LostFocus);
      // 
      // NodeEditor
      // 
      this.Controls.Add(this.txtName);
      this.Name = "NodeEditor";
      this.Size = new System.Drawing.Size(330, 46);
      this.ResumeLayout(false);
    }

    #endregion

    private BorderedTextBox txtName;
  }
}
