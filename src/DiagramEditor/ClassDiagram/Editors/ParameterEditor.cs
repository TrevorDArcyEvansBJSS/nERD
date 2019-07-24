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
using System.Drawing;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
  public sealed partial class ParameterEditor : ItemEditor
  {
    public DelegateShape Shape { get; set; } = null;

    internal override void Init(DiagramElement element)
    {
      Shape = (DelegateShape)element;
      base.Init(element);
    }

    internal override void Relocate(DiagramElement element)
    {
      Relocate((DelegateShape)element);
    }

    internal void Relocate(DelegateShape shape)
    {
      Diagram diagram = shape.Diagram;
      if (diagram != null)
      {
        Rectangle record = shape.GetMemberRectangle(shape.ActiveMemberIndex);

        Point absolute = new Point(shape.Right, record.Top);
        Size relative = new Size(
          (int)(absolute.X * diagram.Zoom) - diagram.Offset.X + MarginSize,
          (int)(absolute.Y * diagram.Zoom) - diagram.Offset.Y);
        relative.Height -= (Height - (int)(record.Height * diagram.Zoom)) / 2;

        Location = ParentLocation + relative;
      }
    }

    protected override void RefreshValues()
    {
      if (Shape.ActiveParameter != null)
      {
        int cursorPosition = SelectionStart;
        DeclarationText = Shape.ActiveParameter.ToString();
        SelectionStart = cursorPosition;

        SetError(null);
        NeedValidation = false;
        RefreshMoveUpDownTools();
      }
    }

    private void RefreshMoveUpDownTools()
    {
      int index = Shape.ActiveMemberIndex;
      int parameterCount = Shape.DelegateType.ArgumentCount;

      toolMoveUp.Enabled = (index > 0);
      toolMoveDown.Enabled = (index < parameterCount - 1);
    }

    protected override bool ValidateDeclarationLine()
    {
      if (NeedValidation && Shape.ActiveParameter != null)
      {
        try
        {
          Shape.DelegateType.ModifyParameter(Shape.ActiveParameter, DeclarationText);
          RefreshValues();
        }
        catch (BadSyntaxException ex)
        {
          SetError(ex.Message);
          return false;
        }
      }
      return true;
    }

    protected override void HideEditor()
    {
      NeedValidation = false;
      Shape.HideEditor();
    }

    protected override void SelectPrevious()
    {
      if (ValidateDeclarationLine())
      {
        Shape.SelectPrevious();
      }
    }

    protected override void SelectNext()
    {
      if (ValidateDeclarationLine())
      {
        Shape.SelectNext();
      }
    }

    protected override void MoveUp()
    {
      if (ValidateDeclarationLine())
      {
        Shape.MoveUp();
      }
    }

    protected override void MoveDown()
    {
      if (ValidateDeclarationLine())
      {
        Shape.MoveDown();
      }
    }

    protected override void Delete()
    {
      Shape.DeleteActiveParameter();
    }
  }
}
