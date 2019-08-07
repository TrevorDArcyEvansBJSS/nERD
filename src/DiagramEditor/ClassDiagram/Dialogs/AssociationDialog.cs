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
using NClass.Translations;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
  public partial class AssociationDialog : Form
  {
    private const int ArrowWidth = 18;
    private const int ArrowHeight = 10;
    private const int DiamondWidth = 20;
    private const int DiamondHeight = 10;

    public AssociationDialog()
    {
      InitializeComponent();
      UpdateTexts();
    }

    private AssociationRelationship _association = null;
    public AssociationRelationship Association
    {
      get
      {
        return _association;
      }
      set
      {
        if (value != null)
        {
          _association = value;
          UpdateFields();
        }
      }
    }

    private void UpdateTexts()
    {
      Text = Strings.EditAssociation;
      btnOK.Text = Strings.ButtonOK;
      btnCancel.Text = Strings.ButtonCancel;
    }

    private void UpdateFields()
    {
      _modifiedDirection = _association.Direction;
      _modifiedType = _association.AssociationType;

      txtFirst.Text = _association.First.Name;
      txtSecond.Text = _association.Second.Name;
      txtName.Text = _association.Label;
      txtStartRole.Text = _association.StartRole;
      txtEndRole.Text = _association.EndRole;
      cboStartMultiplicity.Text = _association.StartMultiplicity;
      cboEndMultiplicity.Text = _association.EndMultiplicity;
    }

    private void ModifyRelationship()
    {
      _association.AssociationType = _modifiedType;
      _association.Direction = _modifiedDirection;
      _association.Label = txtName.Text;
      _association.StartRole = txtStartRole.Text;
      _association.EndRole = txtEndRole.Text;
      _association.StartMultiplicity = cboStartMultiplicity.Text;
      _association.EndMultiplicity = cboEndMultiplicity.Text;
    }

    private void PicArrow_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.X <= DiamondWidth)
      {
        ChangeType();
        picArrow.Invalidate();
      }
      else if (e.X >= picArrow.Width - ArrowWidth)
      {
        ChangeHead();
        picArrow.Invalidate();
      }
    }

    private AssociationType _modifiedType;
    private void ChangeType()
    {
      if (_modifiedType == AssociationType.Association)
      {
        _modifiedType = AssociationType.Aggregation;
      }
      else if (_modifiedType == AssociationType.Aggregation)
      {
        _modifiedType = AssociationType.Composition;
      }
      else
      {
        _modifiedType = AssociationType.Association;
      }
    }

    private Direction _modifiedDirection;
    private void ChangeHead()
    {
      if (_modifiedDirection == Direction.Bidirectional)
      {
        _modifiedDirection = Direction.Unidirectional;
      }
      else
      {
        _modifiedDirection = Direction.Bidirectional;
      }
    }

    private void PicArrow_Paint(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;
      g.SmoothingMode = SmoothingMode.AntiAlias;
      int center = picArrow.Height / 2;
      int width = picArrow.Width;

      // Draw line
      g.DrawLine(Pens.Black, 0, center, width, center);

      // Draw arrow head
      if (_modifiedDirection == Direction.Unidirectional)
      {
        g.DrawLine(Pens.Black, width - ArrowWidth, center - ArrowHeight / 2, width, center);
        g.DrawLine(Pens.Black, width - ArrowWidth, center + ArrowHeight / 2, width, center);
      }

      // Draw start symbol
      if (_modifiedType != AssociationType.Association)
      {
        Point[] diamondPoints =
        {
          new Point(0, center),
          new Point(DiamondWidth / 2, center - DiamondHeight / 2),
          new Point(DiamondWidth, center),
          new Point(DiamondWidth / 2, center + DiamondHeight / 2)
        };

        if (_modifiedType == AssociationType.Aggregation)
        {
          g.FillPolygon(Brushes.White, diamondPoints);
          g.DrawPolygon(Pens.Black, diamondPoints);
        }
        else if (_modifiedType == AssociationType.Composition)
        {
          g.FillPolygon(Brushes.Black, diamondPoints);
        }
      }
    }

    private void BtnOK_Click(object sender, EventArgs e)
    {
      if (_association != null)
      {
        ModifyRelationship();
      }
    }
  }
}
