// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.Translations;
using System;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
  public partial class StateEditor : TypeEditor
  {
    private bool NeedValidation { get; set; } = false;
    private StateShape Shape { get; set; } = null;

    public StateEditor()
    {
      InitializeComponent();

      UpdateTexts();
    }

    internal override void Init(DiagramElement element)
    {
      Shape = (StateShape)element;
      RefreshValues();
    }

    private void UpdateTexts()
    {
    }

    private void RefreshValues()
    {
      SuspendLayout();

      int cursorPosition = txtName.SelectionStart;
      txtName.Text = Shape.Entity.Name;
      txtName.SelectionStart = cursorPosition;

      NeedValidation = false;

      ResumeLayout();
    }

    public override void ValidateData()
    {
      ValidateName();
    }

    private bool ValidateName()
    {
      if (NeedValidation)
      {
        var oldName = Shape.Entity.Name;
        try
        {
          Shape.State.Name = txtName.Text;
          RefreshValues();
        }
        catch (DuplicateTypeException ex)
        {
          Shape.State.Name = oldName;
          MessageBox.Show(ex.Message, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
      }
      return true;
    }

    private void txtName_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Escape:
          NeedValidation = false;
          Shape.HideEditor();
          e.Handled = true;
          break;
      }
    }

    private void txtName_TextChanged(object sender, EventArgs e)
    {
      NeedValidation = true;
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
      base.OnVisibleChanged(e);
      txtName.SelectionStart = 0;
    }
  }
}
