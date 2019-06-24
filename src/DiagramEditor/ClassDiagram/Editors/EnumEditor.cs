﻿// NClass - Free class diagram editor
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

using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
  public partial class EnumEditor : TypeEditor
  {
    private static readonly string newValueText = "« " + Strings.NewValue + " »";

    internal EnumShape Shape { get; set; } = null;
    public bool NeedValidation { get; set; } = false;
    public bool NoNewValue { get; set; } = true;

    public EnumEditor()
    {
      InitializeComponent();
      toolStrip.Renderer = ToolStripSimplifiedRenderer.Default;
      UpdateTexts();
    }

    internal override void Init(DiagramElement element)
    {
      Shape = (EnumShape)element;
      txtNewValue.Text = newValueText;
      NoNewValue = true;
      RefreshValues();
    }

    private void UpdateTexts()
    {
      toolNewValue.ToolTipText = Strings.NewValue;
    }

    private void RefreshValues()
    {
      EnumType type = Shape.EnumType;
      Language language = type.Language;
      SuspendLayout();

      int cursorPosition = txtName.SelectionStart;
      txtName.Text = type.Name;
      txtName.SelectionStart = cursorPosition;

      SetError(null);
      NeedValidation = false;

      RefreshVisibility();
      ResumeLayout();
    }

    private void RefreshVisibility()
    {
      Language language = Shape.EnumType.Language;
      EnumType type = Shape.EnumType;

      toolVisibility.Image = Icons.GetImage(type);
      toolVisibility.Text = language.ValidAccessModifiers[type.AccessModifier];

      // Public
      if (language.ValidAccessModifiers.ContainsKey(AccessModifier.Public))
      {
        toolPublic.Visible = true;
        toolPublic.Text = language.ValidAccessModifiers[AccessModifier.Public];
      }
      else
      {
        toolPublic.Visible = false;
      }
      // Protected Internal
      if (type.IsNested && language.ValidAccessModifiers.ContainsKey(AccessModifier.ProtectedInternal))
      {
        toolProtint.Visible = true;
        toolProtint.Text = language.ValidAccessModifiers[AccessModifier.ProtectedInternal];
      }
      else
      {
        toolProtint.Visible = false;
      }
      // Internal
      if (language.ValidAccessModifiers.ContainsKey(AccessModifier.Internal))
      {
        toolInternal.Visible = true;
        toolInternal.Text = language.ValidAccessModifiers[AccessModifier.Internal];
      }
      else
      {
        toolInternal.Visible = false;
      }
      // Protected
      if (type.IsNested && language.ValidAccessModifiers.ContainsKey(AccessModifier.Protected))
      {
        toolProtected.Visible = true;
        toolProtected.Text = language.ValidAccessModifiers[AccessModifier.Protected];
      }
      else
      {
        toolProtected.Visible = false;
      }
      // Private
      if (type.IsNested && language.ValidAccessModifiers.ContainsKey(AccessModifier.Private))
      {
        toolPrivate.Visible = true;
        toolPrivate.Text = language.ValidAccessModifiers[AccessModifier.Private];
      }
      else
      {
        toolPrivate.Visible = false;
      }
      // Default
      if (language.ValidAccessModifiers.ContainsKey(AccessModifier.Default))
      {
        toolDefault.Visible = true;
        toolDefault.Text = language.ValidAccessModifiers[AccessModifier.Default];
      }
      else
      {
        toolDefault.Visible = false;
      }
    }

    public override void ValidateData()
    {
      ValidateName();
      SetError(null);
    }

    private bool ValidateName()
    {
      if (NeedValidation)
      {
        var oldName = Shape.EnumType.Name;
        try
        {
          Shape.EnumType.Name = txtName.Text;
          RefreshValues();
        }
        catch (BadSyntaxException ex)
        {
          SetError(ex.Message);
          return false;
        }
        catch (DuplicateTypeException ex)
        {
          Shape.EnumType.Name = oldName;
          SetError(ex.Message);
          return false;
        }
      }
      return true;
    }

    private void SetError(string message)
    {
      if (MonoHelper.IsRunningOnMono && MonoHelper.IsOlderVersionThan("2.4"))
        return;

      errorProvider.SetError(this, message);
    }

    private void AddNewValue()
    {
      if (!NoNewValue && ValidateName())
      {
        try
        {
          Shape.EnumType.AddValue(txtNewValue.Text);
          ClearNewValueField();
        }
        catch (BadSyntaxException ex)
        {
          SetError(ex.Message);
        }
      }
    }

    private void ClearNewValueField()
    {
      SetError(null);
      txtNewValue.Text = string.Empty;
    }

    private void ChangeAccess(AccessModifier access)
    {
      if (ValidateName())
      {
        try
        {
          Shape.EnumType.AccessModifier = access;
          RefreshValues();
        }
        catch (BadSyntaxException ex)
        {
          RefreshValues();
          SetError(ex.Message);
        }
      }
    }

    private void toolPublic_Click(object sender, EventArgs e)
    {
      ChangeAccess(AccessModifier.Public);
    }

    private void toolProtint_Click(object sender, EventArgs e)
    {
      ChangeAccess(AccessModifier.ProtectedInternal);
    }

    private void toolInternal_Click(object sender, EventArgs e)
    {
      ChangeAccess(AccessModifier.Internal);
    }

    private void toolProtected_Click(object sender, EventArgs e)
    {
      ChangeAccess(AccessModifier.Protected);
    }

    private void toolPrivate_Click(object sender, EventArgs e)
    {
      ChangeAccess(AccessModifier.Private);
    }

    private void toolDefault_Click(object sender, EventArgs e)
    {
      ChangeAccess(AccessModifier.Default);
    }

    private void txtNewValue_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Enter:
          AddNewValue();
          e.Handled = true;
          break;

        case Keys.Escape:
          NeedValidation = false;
          Shape.HideEditor();
          e.Handled = true;
          break;

        case Keys.Tab:
          txtName.Focus();
          e.Handled = true;
          break;
      }
    }

    private void txtNewValue_TextChanged(object sender, EventArgs e)
    {
      NoNewValue = (txtNewValue.Text.Length == 0);
    }

    private void txtNewValue_GotFocus(object sender, EventArgs e)
    {
      if (NoNewValue)
      {
        txtNewValue.Text = string.Empty;
      }
    }

    private void txtNewValue_LostFocus(object sender, System.EventArgs e)
    {
      if (NoNewValue)
      {
        txtNewValue.Text = newValueText;
        NoNewValue = true;
      }
    }

    private void toolNewValue_Click(object sender, EventArgs e)
    {
      AddNewValue();
    }

    private void txtName_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Enter:
          ValidateName();
          e.Handled = true;
          break;

        case Keys.Escape:
          NeedValidation = false;
          Shape.HideEditor();
          e.Handled = true;
          break;

        case Keys.Down:
          Shape.ActiveMemberIndex = 0;
          e.Handled = true;
          break;
      }
    }

    private void txtName_TextChanged(object sender, EventArgs e)
    {
      NeedValidation = true;
    }

    private void txtName_Validating(object sender, CancelEventArgs e)
    {
      ValidateName();
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
      base.OnVisibleChanged(e);
      txtName.SelectionStart = 0;
    }
  }
}
