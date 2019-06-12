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
using NClass.DiagramEditor.ClassDiagram.Dialogs;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using System;
using System.Drawing;

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
  internal sealed class EntityRelationshipConnection : Connection
  {
    private readonly Size MultiplicitySize = new Size(25, 25);

    private readonly EntityRelationship _relationship;

    public EntityRelationshipConnection(EntityRelationship relationship, Shape startShape, Shape endShape)
      : base(relationship, startShape, endShape)
    {
      _relationship = relationship;
    }

    protected internal override Relationship Relationship
    {
      get { return _relationship; }
    }

    protected override bool IsDashed
    {
      get { return false; }
    }

    protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
    {
      base.OnDoubleClick(e);

      if (!e.Handled)
      {
        ShowEditDialog();
      }
    }

    protected internal override void ShowEditor()
    {
      ShowEditDialog();
    }

    public void ShowEditDialog()
    {
      using (var dialog = new EntityRelationshipDialog(_relationship))
      {
        dialog.ShowDialog();
      }
    }

    protected override bool CloneRelationship(Diagram diagram, Shape first, Shape second)
    {
      var firstType = first.Entity as TypeBase;
      var secondType = second.Entity as TypeBase;

      if (firstType != null && secondType != null)
      {
        var clone = _relationship.Clone(firstType, secondType);
        return diagram.InsertEntityRelationship(clone);
      }
      else
      {
        return false;
      }
    }

    protected override Size StartCapSize
    {
      get
      {
        return MultiplicitySize;
      }
    }

    protected override Size EndCapSize
    {
      get
      {
        return MultiplicitySize;
      }
    }

    public override void Draw(IGraphics g, bool onScreen, Style style)
    {
      base.Draw(g, onScreen, style);

      DrawStartMultiplicity(g, style);
      DrawEndMultiplicity(g, style);
    }

    private void DrawStartMultiplicity(IGraphics g, Style style)
    {
      var startMult = MultiplicityAsString(_relationship.StartMultiplicity);
      DrawMultiplicity(g, style, startMult, RouteCache[0], RouteCache[1], StartCapSize);
    }

    private void DrawEndMultiplicity(IGraphics g, Style style)
    {
      string endMult = MultiplicityAsString(_relationship.EndMultiplicity);
      int last = RouteCache.Count - 1;
      DrawMultiplicity(g, style, endMult, RouteCache[last], RouteCache[last - 1], EndCapSize);
    }

    private void DrawMultiplicity(IGraphics g, Style style, string text, Point firstPoint, Point secondPoint, Size capSize)
    {
      var angle = GetAngle(firstPoint, secondPoint);
      var point = firstPoint;

      using (var textBrush = new SolidBrush(Color.Black))
      {
        using (var stringFormat = new StringFormat(StringFormat.GenericTypographic))
        {
          if (angle == 0) // Down
          {
            point.X += capSize.Width / 2 + TextMargin.Width;
            point.Y += style.ShadowOffset.Height + TextMargin.Height;
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Near;
          }
          else if (angle == 90) // Left
          {
            point.X -= TextMargin.Width;
            point.Y -= capSize.Width / 2 + TextMargin.Height;
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Far;
          }
          else if (angle == 180) // Up
          {
            point.X += capSize.Width / 2 + TextMargin.Width;
            point.Y -= TextMargin.Height;
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Far;
          }
          else // Right
          {
            point.X += style.ShadowOffset.Width + TextMargin.Width;
            point.Y -= capSize.Width / 2 + TextMargin.Height;
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Far;
          }

          textBrush.Color = style.RelationshipTextColor;
          g.DrawString(text, style.RelationshipTextFont, textBrush, point, stringFormat);
        }
      }
    }

    private static string MultiplicityAsString(MultiplicityType mult)
    {
      switch (mult)
      {
        case MultiplicityType.ZeroOrOne:
          return "0..1";
        case MultiplicityType.OneAndOnly:
          return "1";
        case MultiplicityType.ZeroOrMany:
          return "0..*";
        case MultiplicityType.OneOrMany:
          return "1..*";
        default:
          throw new ArgumentOutOfRangeException($"Unknown MultiplicityType: {mult}");
      }
    }

    protected override void DrawStartCap(IGraphics g, bool onScreen, Style style)
    {
      DrawCap(g, onScreen, style, _relationship.StartMultiplicity);
    }

    protected override void DrawEndCap(IGraphics g, bool onScreen, Style style)
    {
      DrawCap(g, onScreen, style, _relationship.EndMultiplicity);
    }

    private static void DrawCap(IGraphics g, bool onScreen, Style style, MultiplicityType multi)
    {
      const int OptionalCircleDiameter = 12;

      using (var pen = new Pen(style.RelationshipColor, style.RelationshipWidth))
      {
        using (var brush = new SolidBrush(Color.White))
        {
          switch (multi)
          {
            case MultiplicityType.ZeroOrOne:
              {
                g.DrawLine(pen, -OptionalCircleDiameter / 2, OptionalCircleDiameter / 2, OptionalCircleDiameter / 2, OptionalCircleDiameter / 2);
                g.FillEllipse(brush, -OptionalCircleDiameter / 2, OptionalCircleDiameter / 2, OptionalCircleDiameter, OptionalCircleDiameter);
                g.DrawEllipse(pen, -OptionalCircleDiameter / 2, OptionalCircleDiameter / 2, OptionalCircleDiameter, OptionalCircleDiameter);
              }
              break;

            case MultiplicityType.OneAndOnly:
              {
                g.DrawLine(pen, -OptionalCircleDiameter / 2, OptionalCircleDiameter, OptionalCircleDiameter / 2, OptionalCircleDiameter);
                g.DrawLine(pen, -OptionalCircleDiameter / 2, OptionalCircleDiameter / 2, OptionalCircleDiameter / 2, OptionalCircleDiameter / 2);
              }
              break;

            case MultiplicityType.ZeroOrMany:
              {
                g.FillEllipse(brush, -OptionalCircleDiameter / 2, OptionalCircleDiameter / 1, OptionalCircleDiameter, OptionalCircleDiameter);
                g.DrawEllipse(pen, -OptionalCircleDiameter / 2, OptionalCircleDiameter / 1, OptionalCircleDiameter, OptionalCircleDiameter);
                var basePt = new Point(0, OptionalCircleDiameter);
                g.DrawLine(pen, basePt, new Point(OptionalCircleDiameter / 2, 0));
                g.DrawLine(pen, basePt, new Point(-OptionalCircleDiameter / 2, 0));
              }
              break;

            case MultiplicityType.OneOrMany:
              {
                g.DrawLine(pen, -OptionalCircleDiameter / 2, OptionalCircleDiameter, OptionalCircleDiameter / 2, OptionalCircleDiameter);
                var basePt = new Point(0, OptionalCircleDiameter);
                g.DrawLine(pen, basePt, new Point(OptionalCircleDiameter / 2, 0));
                g.DrawLine(pen, basePt, new Point(-OptionalCircleDiameter / 2, 0));
              }
              break;

            default:
              throw new ArgumentOutOfRangeException($"Unknown multiplicity: {multi}");
          }
        }
      }
    }
  }
}
