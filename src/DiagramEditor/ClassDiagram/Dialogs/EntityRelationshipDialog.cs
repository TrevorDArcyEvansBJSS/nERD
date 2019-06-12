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
  public partial class EntityRelationshipDialog : Form
  {
    private readonly EntityRelationship _relationship = null;

    public EntityRelationshipDialog(EntityRelationship relationship) :
      this()
    {
      _relationship = relationship;
      UpdateFields();
    }

    public EntityRelationshipDialog()
    {
      InitializeComponent();
      UpdateTexts();
    }

    private void UpdateTexts()
    {
      this.Text = Strings.EditEntityRelationship;
      btnOK.Text = Strings.ButtonOK;
      btnCancel.Text = Strings.ButtonCancel;
    }

    private void UpdateFields()
    {
      txtName.Text = _relationship.Label;
      cboStartMultiplicity.SelectedIndex = (int)_relationship.StartMultiplicity;
      cboEndMultiplicity.SelectedIndex = (int)_relationship.EndMultiplicity;
      picArrow.Invalidate();
    }

    private void ModifyRelationship()
    {
      _relationship.Label = txtName.Text;
      _relationship.StartMultiplicity = (MultiplicityType)cboStartMultiplicity.SelectedIndex;
      _relationship.EndMultiplicity = (MultiplicityType)cboEndMultiplicity.SelectedIndex;
    }

    private void PicArrow_Paint(object sender, PaintEventArgs e)
    {
      const int MultiplicityWidth = 25;

      var g = e.Graphics;
      g.SmoothingMode = SmoothingMode.AntiAlias;

      var center = picArrow.Height / 2;
      var width = picArrow.Width;

      // Draw line
      g.DrawLine(Pens.Black, MultiplicityWidth, center, width - MultiplicityWidth, center);

      using (var brush = new SolidBrush(Color.Black))
      {
        using (var font = new Font("Courier New", 9F, FontStyle.Regular))
        {
          // Draw StartMultiplicity
          var startMult = StartMultiplicityAsString((MultiplicityType)cboStartMultiplicity.SelectedIndex);
          var startMultSize = g.MeasureString(startMult, font);
          g.DrawString(startMult, font, brush,
            MultiplicityWidth - startMultSize.Width, center - startMultSize.Height / 2);

          // Draw EndMultiplicity
          var endMult = EndMultiplicityAsString((MultiplicityType)cboEndMultiplicity.SelectedIndex);
          var endMultSize = g.MeasureString(endMult, font);
          g.DrawString(endMult, font, brush,
            width - MultiplicityWidth, center - endMultSize.Height / 2);
        }
      }
    }

    private static string StartMultiplicityAsString(MultiplicityType mult)
    {
      switch (mult)
      {
        case MultiplicityType.ZeroOrOne:
          return "o+";
        case MultiplicityType.OneAndOnly:
          return "++";
        case MultiplicityType.ZeroOrMany:
          return ">o";
        case MultiplicityType.OneOrMany:
          return ">+";
        default:
          throw new ArgumentOutOfRangeException($"Unknown MultiplicityType: {mult}");
      }
    }

    private static string EndMultiplicityAsString(MultiplicityType mult)
    {
      switch (mult)
      {
        case MultiplicityType.ZeroOrOne:
          return "+o";
        case MultiplicityType.OneAndOnly:
          return "++";
        case MultiplicityType.ZeroOrMany:
          return "o<";
        case MultiplicityType.OneOrMany:
          return "+<";
        default:
          throw new ArgumentOutOfRangeException($"Unknown MultiplicityType: {mult}");
      }
    }

    private void BtnOK_Click(object sender, EventArgs e)
    {
      ModifyRelationship();
    }

    private void CboStartMultiplicity_SelectedIndexChanged(object sender, EventArgs e)
    {
      picArrow.Invalidate();
    }

    private void CboEndMultiplicity_SelectedIndexChanged(object sender, EventArgs e)
    {
      picArrow.Invalidate();
    }
  }
}
